using NUnit.Framework;
using TCGPlayerPriceScrapper_PlaywrightVersion.Test_Data;
using TCGPlayerPriceScrapper_PlaywrightVersion.Utilities;

[assembly: Parallelizable(ParallelScope.Children)]
[assembly: LevelOfParallelism(4)]

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Tests
{
    public class Tests : Base
    {
        [Test, TestCaseSource(typeof(ItemsToPrice_DataProvider), nameof(ItemsToPrice_DataProvider.ListOfItems))]
        public async Task GetItemPrice(int sortIndex, string barCode, string itemNumber, string itemName, string itemURL, decimal minPriceUsd, decimal discount)
        {
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

                if (itemMarketPrice > (basePriceVal + shippingVal))
                {
                    basePriceVal = itemMarketPrice;
                    shippingVal = 0;
                }

                decimal totalUsd = basePriceVal + shippingVal;

                // Guarda los datos sin procesar junto con el Sort Index
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
            finally
            {
                await context.CloseAsync();
            }
        }
    }
}