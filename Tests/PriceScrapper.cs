using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using TCGPlayerPriceScrapper_PlaywrightVersion.Test_Data;
using TCGPlayerPriceScrapper_PlaywrightVersion.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Tests
{
    public class Tests : Base
    {
        [Test, TestCaseSource(typeof(ItemsToPrice_DataProvider), nameof(ItemsToPrice_DataProvider.ListOfItems))]
        public async Task GetItemPrice(string barCode, string itemNumber, string itemName, string itemURL)
        {
            IPage currentPage = ReturnPage();
            await currentPage.GotoAsync(itemURL);

            string currentItemPrice = await GetItemPrice(currentPage);
            string currentItemShippingPrice = await GetItemShippingPrice(currentPage);
            string currentListingsAvailable = await GetOtherListings(currentPage);
            string currentItemMarketPrice = await GetItemMarketPrice(currentPage);


            // Cálculo opcional para Precio Total USD (Precio Base + Envío)
            decimal basePriceVal = decimal.TryParse(currentItemPrice.Replace("$", "").Trim(), out var bp) ? bp : 0;
            decimal shippingVal = decimal.TryParse(currentItemShippingPrice.Replace("$", "").Trim(), out var sp) ? sp : 0;
            decimal itemMarketPrice = decimal.TryParse(currentItemMarketPrice.Replace("$", "").Trim(), out var mkt) ? mkt : 0;

            //Si el Market Price del producto es mayor a TCGLow. 
            //Usaremos el precio de Market Price para vender el producto.
            if (itemMarketPrice > (basePriceVal+shippingVal))
            {
                basePriceVal = itemMarketPrice;
                shippingVal = 0;
            }

            decimal totalUsd = basePriceVal + shippingVal;

            // Añadiendo el registro al buffer de Google Sheets en orden de las 9 columnas (A a I):
            // A: UPC, B: Item#, C: Producto, D: Precio Base, E: Envío, F: Total USD, G: Total MXN, H: Precio Compra, I: Enlace
            sheetRows.Add(new List<object> {
                barCode,                      // A: Código de Escáner (UPC / Barcode)
                itemNumber,                   // B: TCGplayer Item #
                itemName,                     // C: Producto Sellado
                currentItemPrice,             // D: Precio Base TCG (USD)
                currentItemShippingPrice,     // E: Envío / Shipping (USD)
                totalUsd.ToString("F2"),      // F: Precio Total USD
                "",                           // G: Precio Total MXN
                "",                           // H: Precio de Compra USD
                itemURL                       // I: Enlace TCGplayer
            });
        }
    }
}