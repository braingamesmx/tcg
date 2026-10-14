using NUnit.Framework;
using TCGPlayerPriceScrapper_PlaywrightVersion.Test_Data;
using TCGPlayerPriceScrapper_PlaywrightVersion.Utilities;

[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(1)]

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Tests
{
    public class Tests : Base
    {
        [Test, TestCaseSource(typeof(ItemsToPrice_DataProvider), nameof(ItemsToPrice_DataProvider.ListOfItems))]
        public async Task GetItemPrice(
            int sortIndex,
            string barCode,
            string itemNumber,
            string itemName,
            string itemURL,
            decimal minPriceUsd,
            decimal discount,
            string isDynamic) // 👈 ADDED 8TH PARAMETER HERE
        {
            string logHeader = $"[Item #{itemNumber}] {itemName}";

            // IF PRECIO DINAMICO IS "NO": Bypass browser scraping completely
            if (string.Equals(isDynamic, "No", StringComparison.OrdinalIgnoreCase))
            {
                WriteLog($"{logHeader} | Precio Dinamico = 'No'. Bypassed TCGPlayer web scraping. Using Min USD (${minPriceUsd:F2}).");

                string currentItemPrice = "$0.00";
                string currentItemShippingPrice = "$0.00";
                decimal totalUsd = minPriceUsd;

                AddPendingRow(sortIndex, new List<object> {
                    barCode,                  // 0
                    itemNumber,               // 1
                    itemName,                 // 2
                    currentItemPrice,         // 3: Column D ($0.00)
                    currentItemShippingPrice, // 4: Column E ($0.00)
                    totalUsd.ToString("F2"),  // 5: Column F (Total USD = minPriceUsd)
                    itemURL,                  // 6
                    minPriceUsd,              // 7
                    discount                  // 8
                });

                return;
            }

            // IF PRECIO DINAMICO IS "YES": Perform standard Playwright scraping
            var (context, currentPage) = await CreateIsolatedPageAsync();

            try
            {
                await currentPage.GotoAsync(itemURL);

                string currentItemPrice = await GetItemPrice(currentPage);
                string currentItemShippingPrice = await GetItemShippingPrice(currentPage);
                string currentListingsAvailable = await GetOtherListings(currentPage);
                string currentItemMarketPrice = await GetItemMarketPrice(currentPage);

                decimal basePriceVal = decimal.TryParse(currentItemPrice.Replace("$", "").Trim(), out var bp) ? bp : 0;
                decimal shippingVal = decimal.TryParse(currentItemShippingPrice.Replace("$", "").Trim(), out var sp) ? sp : 0;
                decimal itemMarketPrice = decimal.TryParse(currentItemMarketPrice.Replace("$", "").Trim(), out var mkt) ? mkt : 0;

                WriteLog($"{logHeader} | Scraped Raw -> Base: '{currentItemPrice}', Shipping: '{currentItemShippingPrice}', Market: '{currentItemMarketPrice}'");

                if (basePriceVal == 0)
                {
                    WriteLog($"  [WARNING] Spotlight Base Price was '$0' or timed out.");
                }

                if (itemMarketPrice > (basePriceVal + shippingVal))
                {
                    WriteLog($"  [OVERRIDE] Market Price (${itemMarketPrice:F2}) > Scraped Total (${basePriceVal + shippingVal:F2}). Overriding calculation to Market Price.");
                    basePriceVal = itemMarketPrice;
                    shippingVal = 0;
                }

                decimal totalUsd = basePriceVal + shippingVal;

                AddPendingRow(sortIndex, new List<object> {
                    barCode,                  // 0
                    itemNumber,               // 1
                    itemName,                 // 2
                    currentItemPrice,         // 3
                    currentItemShippingPrice, // 4
                    totalUsd.ToString("F2"),  // 5
                    itemURL,                  // 6
                    minPriceUsd,              // 7
                    discount                  // 8
                });
            }
            catch (Exception ex)
            {
                WriteLog($"[ERROR] Failed processing Item #{itemNumber}: {ex.Message}");
            }
            finally
            {
                await context.CloseAsync();
            }
        }
    }
}