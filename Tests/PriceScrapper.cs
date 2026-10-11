using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Playwright;
using NUnit.Framework;
using TCGPlayerPriceScrapper_PlaywrightVersion.Test_Data;
using TCGPlayerPriceScrapper_PlaywrightVersion.Utilities;

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Tests
{
    public class Tests : Base
    {
        [Test, TestCaseSource(typeof(ItemsToPrice_DataProvider), nameof(ItemsToPrice_DataProvider.ListOfItems))]
        public async Task GetItemPrice(string barCode, string itemNumber, string itemName, string itemURL, decimal minPriceUsd, decimal discount)
        {
            IPage currentPage = ReturnPage();
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

            // El buffer ahora contiene exactamente 11 columnas (A a K).
            sheetRows.Add(new List<object> {
                barCode,                                          // A: Código de Escáner (UPC / Barcode)
                itemNumber,                                       // B: TCGplayer Item #
                itemName,                                         // C: Producto Sellado
                currentItemPrice,                                 // D: Precio Base TCG (USD)
                currentItemShippingPrice,                         // E: Envío / Shipping (USD)
                totalUsd.ToString("F2"),                          // F: Precio Total USD
                $"=F{NextRowIndex}*$C$1*(1 - K{NextRowIndex})",   // G: Precio Total MXN (Con Descuento Aplicado)
                itemURL,                                          // H: Enlace TCGplayer
                minPriceUsd,                                      // I: Precio Minimo de Venta (USD)
                $"=I{NextRowIndex}*$C$1",                         // J: PrecioPesosMinimo de Venta (MXN Directo)
                discount                                          // K: Descuento Actual (e.g. 0.15 para 15%)
            });

            NextRowIndex++;
        }
    }
}