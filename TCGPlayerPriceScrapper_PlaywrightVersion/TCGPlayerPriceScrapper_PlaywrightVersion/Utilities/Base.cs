using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Microsoft.Playwright;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Utilities
{
    public class Base
    {
        public IPage page;
        public IBrowser browser;
        public List<IList<object>> sheetRows = new List<IList<object>>(); // Buffer para los datos
        public string timeStamp;
        Stopwatch stopwatch;

        // Configuración de tu Google Sheet
        private const string SpreadsheetId = "1xK3muMQsNsTTOC3w64AvusEHG79r_6q5yo0c1zgmAdU";
        private const string SheetRange = "Reality Fracture Tracker!A:I";
        private const string CredentialsFilePath = "credentials.json";

        /// <summary>
        /// Inicializa el cronómetro de ejecución.
        /// </summary>
        [OneTimeSetUp]
        public void CreateSheetHeader()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();  // Start timer for execution.  
        }

        /// <summary>
        /// Envía todos los registros acumulados y métricas de tiempo a Google Sheets al finalizar las pruebas.
        /// </summary>
        [OneTimeTearDown]
        public void CollectAllData()
        {
            timeStamp = CreateTimeStamp();
            stopwatch.Stop(); // Stop timer of execution.
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine(); Console.WriteLine(); //double enter
            Console.WriteLine("Elapsed Time is {0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            string executionTime = ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;

            // Fila de cierre con el tiempo total (opcional)
            sheetRows.Add(new List<object> { "999", "Execution Complete", "Created on " + timeStamp, "", "", "", executionTime, "", "" });

            // Envía los datos mediante la API de Google Sheets
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

            var valueRange = new ValueRange
            {
                Values = sheetRows
            };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, SheetRange);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }

        [SetUp]
        public async Task SetupBrowser()
        {
            IPlaywright playwright = await Playwright.CreateAsync();
            var chrome = playwright.Chromium;
            bool runHeadless = false; // Toggle this to true or false whenever you want

            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = runHeadless,
                Args = runHeadless
                    ? new[] { "--headless=new", "--disable-blink-features=AutomationControlled" }
                    : new[] { "--disable-blink-features=AutomationControlled" } // Remove the headless flag when visible
            });

            var context = await browser.NewContextAsync(new BrowserNewContextOptions
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }, // Prevents default 0x0 headless window size
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
        public async Task TearDownBrowser()
        {
            await browser.CloseAsync();
        }

        public IPage ReturnPage()
        {
            return page;
        }

        public IBrowser ReturnBrowser()
        {
            return browser;
        }

        public static string CreateTimeStamp()
        {
            // Generate TimeStamp
            var currentTime = DateTime.Now;
            string myDate = currentTime.ToString("yyyy_MMM_dd");
            string myTime = currentTime.ToLongTimeString();
            myTime = myTime.Replace(":", ".");
            string timeStamp = myDate + " " + myTime;
            return timeStamp;
        }
    }
}