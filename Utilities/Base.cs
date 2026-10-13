using System.Diagnostics;
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
        protected static IPlaywright playwright;
        protected static IBrowser browser;

        protected static readonly object _sheetLock = new object();
        private static readonly object _logLock = new object();

        public List<(int SortIndex, IList<object> RowData)> pendingRows = new List<(int, IList<object>)>();
        public List<IList<object>> sheetRows = new List<IList<object>>();

        public string timeStamp;
        Stopwatch stopwatch;

        protected const string SpreadsheetId = "1xK3muMQsNsTTOC3w64AvusEHG79r_6q5yo0c1zgmAdU";
        protected const string SheetRange = "SealedItemsPriceTracker!A4:K";
        protected const string CredentialsFilePath = "credentials.json";
        protected static string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "execution.log");

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();

            // Overwrite log file for a fresh execution run
            lock (_logLock)
            {
                File.WriteAllText(LogFilePath, $"=== TCGPlayer Scraper Log Run Started: {DateTime.Now} ===\n\n");
            }

            ClearStaleSheetData();

            playwright = await Playwright.CreateAsync();
            bool runHeadless = true;

            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = runHeadless,
                Args = runHeadless
                    ? new[] { "--headless=new", "--disable-blink-features=AutomationControlled" }
                    : new[] { "--disable-blink-features=AutomationControlled" }
            });
        }

        public static void WriteLog(string message)
        {
            lock (_logLock)
            {
                File.AppendAllText(LogFilePath, $"[{DateTime.Now:HH:mm:ss}] {message}\n");
            }
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if (browser != null) await browser.CloseAsync();
            playwright?.Dispose();

            var sortedRows = pendingRows.OrderBy(r => r.SortIndex).ToList();

            int rowIndex = 4;
            foreach (var item in sortedRows)
            {
                var d = item.RowData;
                sheetRows.Add(new List<object> {
                    d[0],                                           // A: Barcode
                    d[1],                                           // B: Item #
                    d[2],                                           // C: Nombre
                    d[3],                                           // D: Base USD
                    d[4],                                           // E: Shipping USD
                    d[5],                                           // F: Total USD
                    $"=F{rowIndex}*$C$1*(1 - K{rowIndex})",         // G: Total MXN Formula
                    d[6],                                           // H: URL
                    d[7],                                           // I: Min USD
                    $"=I{rowIndex}*$C$1",                           // J: Min MXN Formula
                    d[8]                                            // K: Descuento
                });

                rowIndex++;
            }

            timeStamp = CreateTimeStamp();
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            WriteLog($"=== Execution Finished in {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds} ===");

            sheetRows.Add(new List<object> { "999", "Execution Complete", "Created on " + timeStamp, "", "", "", ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds, "", "", "", "" });

            PushDataToGoogleSheet();
        }

        public async Task<(IBrowserContext Context, IPage Page)> CreateIsolatedPageAsync()
        {
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

            var page = await context.NewPageAsync();
            return (context, page);
        }

        public void AddPendingRow(int sortIndex, IList<object> rawRow)
        {
            lock (_sheetLock)
            {
                pendingRows.Add((sortIndex, rawRow));
            }
        }

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

                string rangeToClear = "SealedItemsPriceTracker!A4:K2000";
                var clearRequest = service.Spreadsheets.Values.Clear(new ClearValuesRequest(), SpreadsheetId, rangeToClear);
                clearRequest.Execute();

                WriteLog("Stale rows wiped from Tracker (Columns A to K down to row 2000).");
            }
            catch (Exception ex)
            {
                WriteLog($"Warning: Failed to clear automated sheet columns: {ex.Message}");
            }
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
            WriteLog("Successfully updated Google Sheet with scraped data.");
        }

        public static string CreateTimeStamp()
        {
            var currentTime = DateTime.Now;
            string myDate = currentTime.ToString("yyyy_MMM_dd");
            string myTime = currentTime.ToLongTimeString().Replace(":", ".");
            return myDate + " " + myTime;
        }

        public async Task<string> GetItemPrice(IPage currentPage)
        {
            try
            {
                ILocator itemPrice = currentPage.Locator("//*[@class='spotlight__price']");
                await itemPrice.WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
                return await itemPrice.IsVisibleAsync() ? await itemPrice.InnerTextAsync() : "$0";
            }
            catch { return "$0"; }
        }

        public async Task<string> GetItemShippingPrice(IPage currentPage)
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

        public async Task<string> GetOtherListings(IPage currentPage)
        {
            try
            {
                ILocator listingsAvailable = currentPage.Locator("[class^='view-all-listings__other-listings']");
                await listingsAvailable.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
                return await listingsAvailable.IsVisibleAsync() ? await listingsAvailable.InnerTextAsync() : "No other listings";
            }
            catch { return "No other listings"; }
        }

        public async Task<string> GetItemMarketPrice(IPage currentPage)
        {
            try
            {
                ILocator currentMarketPrice = currentPage.Locator("//tr[td[text()='Market Price']]/td[2]");
                await currentMarketPrice.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
                return await currentMarketPrice.IsVisibleAsync() ? await currentMarketPrice.InnerTextAsync() : "0";
            }
            catch { return "0"; }
        }
    }
}