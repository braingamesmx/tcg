using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Playwright;
using NUnit.Framework;

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Utilities
{
    public class Base
    {
        public IPage page;
        public IBrowser browser;
        public List<IList<object>> sheetRows = new List<IList<object>>(); // Buffer para los datos
        public string timeStamp;
        Stopwatch stopwatch;

        // Tracks rows dynamically starting at Row 4
        public int NextRowIndex = 4;

        // Configuración de tu Google Sheet (Restringido a las columnas de la A a la K)
        protected const string SpreadsheetId = "1xK3muMQsNsTTOC3w64AvusEHG79r_6q5yo0c1zgmAdU";
        protected const string SheetRange = "SealedItemsPriceTracker!A4:K";
        protected const string CredentialsFilePath = "credentials.json";

        /// <summary>
        /// Inicializa el cronómetro de ejecución y limpia los datos viejos de las columnas automatizadas.
        /// </summary>
        [OneTimeSetUp]
        public void CreateSheetHeader()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            // Limpia únicamente el rango automatizado (Columnas A-K)
            ClearStaleSheetData();
        }

        /// <summary>
        /// Limpia el rango principal de la automatización (A4:K).
        /// </summary>
        private void ClearStaleSheetData()
        {
            try
            {
                GoogleCredential credential;
                using (var stream = new FileStream(CredentialsFilePath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
                }

                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "TCGPlayer Price Scrapper"
                });

                // Limpia desde la fila 4 hacia abajo cruzando de la A hasta la K
                string rangeToClear = "SealedItemsPriceTracker!A4:K";
                var clearRequest = service.Spreadsheets.Values.Clear(new ClearValuesRequest(), SpreadsheetId, rangeToClear);
                clearRequest.Execute();

                Console.WriteLine("Stale rows wiped from Tracker (Columns A to K).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to clear automated sheet columns: {ex.Message}");
            }
        }

        /// <summary>
        /// Envía todos los registros acumulados y métricas de tiempo a Google Sheets al finalizar las pruebas.
        /// </summary>
        [OneTimeTearDown]
        public void CollectAllData()
        {
            timeStamp = CreateTimeStamp();
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Elapsed Time is {0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            string executionTime = ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;

            // Fila final ajustada a 11 columnas de ancho (A a K)
            sheetRows.Add(new List<object> { "999", "Execution Complete", "Created on " + timeStamp, "", "", "", executionTime, "", "", "", "" });

            PushDataToGoogleSheet();
        }

        private void PushDataToGoogleSheet()
        {
            GoogleCredential credential;
            using (var stream = new FileStream(CredentialsFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "TCGPlayer Price Scrapper"
            });

            var valueRange = new ValueRange { Values = sheetRows };
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, SheetRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }

        [SetUp]
        public async Task SetupBrowser()
        {
            IPlaywright playwright = await Playwright.CreateAsync();
            bool runHeadless = true;

            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = runHeadless,
                Args = runHeadless
                    ? new[] { "--headless=new", "--disable-blink-features=AutomationControlled" }
                    : new[] { "--disable-blink-features=AutomationControlled" }
            });

            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
                ExtraHTTPHeaders = new Dictionary<string, string>
                {
                    { "sec-ch-ua", @"""Not_A Brand"";v=""8"", ""Chromium"";v=""120"", ""Google Chrome"";v=""120""" },
                    { "sec-ch-ua-mobile", "?0" },
                    { "sec-ch-ua-platform", @"""Windows""" }
                }
            });

            page = await context.NewPageAsync();
        }

        [TearDown]
        public async Task TearDownBrowser() => await browser.CloseAsync();
        public IPage ReturnPage() => page;
        public IBrowser ReturnBrowser() => browser;

        public static string CreateTimeStamp()
        {
            var currentTime = DateTime.Now;
            string myDate = currentTime.ToString("yyyy_MMM_dd");
            string myTime = currentTime.ToLongTimeString().Replace(":", ".");
            return myDate + " " + myTime;
        }

        public async Task<String> GetItemPrice(IPage currentPage)
        {
            try
            {
                ILocator itemPrice = currentPage.Locator("//*[@class='spotlight__price']");
                await itemPrice.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
                return await itemPrice.IsVisibleAsync() ? await itemPrice.InnerTextAsync() : "$0";
            }
            catch
            {
                return "$0";
            }
        }

        public async Task<String> GetItemShippingPrice(IPage currentPage)
        {
            try
            {
                ILocator shippingPrice = currentPage.Locator("//*[@class='spotlight__shipping']");
                await shippingPrice.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
                if (await shippingPrice.IsVisibleAsync())
                {
                    string shipText = await shippingPrice.InnerTextAsync();
                    if (shipText.Trim().Equals("Shipping: Included")) return "0";
                    Match match = Regex.Match(shipText, @"\$([0-9]+\.[0-9]{2})");
                    return match.Groups[1].Value;
                }
            }
            catch { }
            return "0";
        }

        public async Task<String> GetOtherListings(IPage currentPage)
        {
            try
            {
                ILocator listingsAvailable = currentPage.Locator("[class^='view-all-listings__other-listings']");
                await listingsAvailable.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
                return await listingsAvailable.IsVisibleAsync() ? await listingsAvailable.InnerTextAsync() : "No other listings";
            }
            catch
            {
                return "No other listings";
            }
        }

        public async Task<String> GetItemMarketPrice(IPage currentPage)
        {
            try
            {
                ILocator currentMarketPrice = currentPage.Locator("//tr[td[text()='Market Price']]/td[2]");
                await currentMarketPrice.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
                return await currentMarketPrice.IsVisibleAsync() ? await currentMarketPrice.InnerTextAsync() : "0";
            }
            catch
            {
                return "0";
            }
        }
    }
}