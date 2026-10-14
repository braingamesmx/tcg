using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using NUnit.Framework;

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Test_Data
{
    internal class ItemsToPrice_DataProvider
    {
        private const string InventorySpreadsheetId = "1cHg5Qjb2uZrOVMmN_Wz496PS8QwHCqApCJqQORwVLDA";

        // Updated range to read through Column G
        private const string InventorySheetRange = "'Items Disponibles'!A2:G";

        public static IEnumerable<TestCaseData> ListOfItems()
        {
            var itemsList = new List<TestCaseData>();

            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string credentialsPath = Path.Combine(baseDir, "credentials.json");

                if (!File.Exists(credentialsPath))
                {
                    credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "credentials.json");
                }

                if (!File.Exists(credentialsPath))
                {
                    TestContext.Progress.WriteLine($"CRITICAL ERROR: credentials.json no fue encontrado en: {credentialsPath}");
                }
                else
                {
                    GoogleCredential credential;
#pragma warning disable CS0618
                    using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
                    }
#pragma warning restore CS0618

                    var service = new SheetsService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "TCGPlayer Price Scrapper"
                    });

                    var request = service.Spreadsheets.Values.Get(InventorySpreadsheetId, InventorySheetRange);
                    var response = request.Execute();
                    var values = response.Values;

                    if (values != null && values.Count > 0)
                    {
                        int sortIndex = 0;

                        foreach (var row in values)
                        {
                            if (row.Count < 3) continue;

                            string barCode = row.Count > 0 ? row[0]?.ToString()?.Trim() ?? "" : "";
                            string itemNumber = row.Count > 1 ? row[1]?.ToString()?.Trim() ?? "" : "";
                            string itemName = row.Count > 2 ? row[2]?.ToString()?.Trim() ?? "" : "";

                            if (string.IsNullOrEmpty(itemNumber)) continue;

                            string itemURL = $"https://www.tcgplayer.com/product/{itemNumber}";

                            // Min Price (Col E / Index 4)
                            decimal minPriceUsd = 0m;
                            if (row.Count > 4 && row[4] != null)
                            {
                                string rawMinPrice = row[4].ToString().Replace("$", "").Replace(",", "").Trim();
                                decimal.TryParse(rawMinPrice, out minPriceUsd);
                            }

                            // Discount (Col F / Index 5)
                            decimal discount = 0m;
                            if (row.Count > 5 && row[5] != null)
                            {
                                string rawDiscount = row[5].ToString().Replace("%", "").Trim();
                                if (decimal.TryParse(rawDiscount, out var parsedDiscount))
                                {
                                    discount = parsedDiscount > 1 ? parsedDiscount / 100m : parsedDiscount;
                                }
                            }

                            // Precio Dinamico (Col H / Index 7)
                            string isDynamic = row.Count > 6 ? row[6]?.ToString()?.Trim() ?? "Yes" : "Yes";

                            itemsList.Add(new TestCaseData(
                                sortIndex,
                                barCode,
                                itemNumber,
                                itemName,
                                itemURL,
                                minPriceUsd,
                                discount,
                                isDynamic
                            ));

                            sortIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"API ERROR al leer inventario: {ex.Message} | StackTrace: {ex.StackTrace}");
            }

            if (itemsList.Count == 0)
            {
                itemsList.Add(new TestCaseData(0, "196214158801", "704143", "30th Celebration Elite Trainer Box", "https://www.tcgplayer.com/product/704143", 0.00m, 0.15m, "Yes"));
            }

            foreach (var item in itemsList)
            {
                yield return item;
            }
        }
    }
}