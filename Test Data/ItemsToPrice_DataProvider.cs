using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCGPlayerPriceScrapper_PlaywrightVersion.Test_Data
{
    internal class ItemsToPrice_DataProvider
    {
        public static IEnumerable<TestCaseData> ListOfItems()
        {
            yield return new TestCaseData("UPC Code", "704143", "30th Celebration Elite Trainer Box", "https://www.tcgplayer.com/product/704143");
            yield return new TestCaseData("UPC Code", "704168", "30th Celebration Greninja ex Box", "https://www.tcgplayer.com/product/704168");
            yield return new TestCaseData("UPC Code", "716515", "Poke Pad - ME: 30th Celebration (30C)", "https://www.tcgplayer.com/product/716515");

        }
    }
}

