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
        public async Task GetItemPrice(string itemNumber, string itemName, string itemURL)
        {
            IPage currentPage = ReturnPage();
            ILocator itemPrice = currentPage.Locator("//*[@class='spotlight__price']");
            ILocator listingsAvailable = currentPage.Locator("[class^='view-all-listings__other-listings']");
            ILocator shippingPrice = currentPage.Locator("//*[@class='spotlight__shipping']");

            await currentPage.GotoAsync(itemURL);

            string currentItemPrice = "";
            string currentListingsAvailable = "";
            string currentItemShippingPrice = "";

            // Getting item price for current item.
            await itemPrice.WaitForAsync();
            if (await itemPrice.IsVisibleAsync())
            {
                await itemPrice.WaitForAsync();
                currentItemPrice = await itemPrice.InnerTextAsync();
            }
            else
            {
                currentItemPrice = "$0";
            }

            // Getting Shipping Price (Lógica exacta original)
            await shippingPrice.WaitForAsync();
            if (await shippingPrice.IsVisibleAsync())
            {
                await shippingPrice.WaitForAsync();
                currentItemShippingPrice = await shippingPrice.InnerTextAsync();

                if (currentItemShippingPrice.Trim().Equals("Shipping: Included"))
                    currentItemShippingPrice = "0";
                else
                {
                    Match match = Regex.Match(currentItemShippingPrice, @"\$([0-9]+\.[0-9]{2})");
                    currentItemShippingPrice = match.Groups[1].Value;
                }
            }

            // Getting item listings available
            await listingsAvailable.WaitForAsync();
            if (await listingsAvailable.IsVisibleAsync())
            {
                await listingsAvailable.WaitForAsync();
                currentListingsAvailable = await listingsAvailable.InnerTextAsync();
            }
            else
            {
                currentListingsAvailable = "No other listings";
            }

            // Cálculo opcional para Precio Total USD (Precio Base + Envío)
            decimal basePriceVal = decimal.TryParse(currentItemPrice.Replace("$", "").Trim(), out var bp) ? bp : 0;
            decimal shippingVal = decimal.TryParse(currentItemShippingPrice.Replace("$", "").Trim(), out var sp) ? sp : 0;
            decimal totalUsd = basePriceVal + shippingVal;

            // Añadiendo el registro al buffer de Google Sheets en orden de las 9 columnas (A a I):
            // A: UPC, B: Item#, C: Producto, D: Precio Base, E: Envío, F: Total USD, G: Total MXN, H: Precio Compra, I: Enlace
            sheetRows.Add(new List<object> {
                "",                           // A: Código de Escáner (UPC / Barcode)
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