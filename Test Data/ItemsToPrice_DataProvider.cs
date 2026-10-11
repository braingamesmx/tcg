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


            // **************************   ************  Datos para Pokemon
            yield return new TestCaseData("196214158849", "704171", "30th Celebration Booster Bundle", "https://www.tcgplayer.com/product/704171", 45.00m, 0.15m);
            yield return new TestCaseData("196214158801", "704143", "30th Celebration Elite Trainer Box", "https://www.tcgplayer.com/product/704143", 85.00m, 0.15m);
            yield return new TestCaseData("196214158726", "704168", "30th Celebration Greninja ex Box", "https://www.tcgplayer.com/product/704168", 30.00m, 0.15m);
            yield return new TestCaseData("196214158757", "704153", "30th Celebration Poster Collection", "https://www.tcgplayer.com/product/704153", 30.00m, 0.15m);
            yield return new TestCaseData("196214158771", "704146", "30th Celebration Tech Sticker Collection", "https://www.tcgplayer.com/product/704146", 20.00m, 0.15m);
            yield return new TestCaseData("196214131781", "666906", "Ascended Heroes Collection - Erika (3-Tab)", "https://www.tcgplayer.com/product/666906", 15.00m, 0.00m);
            yield return new TestCaseData("196214132474", "668496", "Ascended Heroes Elite Trainer Box", "https://www.tcgplayer.com/product/668496", 90.00m, 0.00m);
            yield return new TestCaseData("196214129238", "630430", "Black Bolt Binder Collection", "https://www.tcgplayer.com/product/630430", 55.00m, 0.00m);
            yield return new TestCaseData("820650809385", "242807", "Celebrations Collection Dragapult Prime", "https://www.tcgplayer.com/product/242807", 90.00m, 0.00m);
            yield return new TestCaseData("820650809439", "242811", "Celebrations Elite Trainer Box", "https://www.tcgplayer.com/product/242811", 300.00m, 0.00m);
            yield return new TestCaseData("196214154155", "684458", "Chaos Rising 3-Pack Blister", "https://www.tcgplayer.com/product/684458", 17.00m, 0.00m);
            yield return new TestCaseData("196214154186", "684444", "Chaos Rising Booster Box", "https://www.tcgplayer.com/product/684444", 163.00m, 0.00m);
            yield return new TestCaseData("196214154162", "684456", "Chaos Rising Booster Bundle", "https://www.tcgplayer.com/product/684456", 28.00m, 0.00m);
            yield return new TestCaseData("196214154117", "684450", "Chaos Rising Elite Trainer Box", "https://www.tcgplayer.com/product/684450", 55.00m, 0.00m);
            yield return new TestCaseData("196214150522", "673436", "First Partner Illustration Collection Series 1", "https://www.tcgplayer.com/product/673436", 25.00m, 0.00m);
            yield return new TestCaseData("196214155978", "688712", "First Partner Illustration Collection Series 2", "https://www.tcgplayer.com/product/688712", 20.00m, 0.00m);
            yield return new TestCaseData("196214135079", "684106", "League Battle Deck Mega Lucario ex", "https://www.tcgplayer.com/product/684106", 15.00m, 0.00m);
            yield return new TestCaseData("196214147539", "671250", "Mega Charizard Tin", "https://www.tcgplayer.com/product/671250", 25.00m, 0.00m);
            yield return new TestCaseData("196214130685", "654213", "Mega Charizard X ex Ultra Premium Collection", "https://www.tcgplayer.com/product/654213", 200.00m, 0.00m);
            yield return new TestCaseData("196214114845", "644279", "Mega Evolution Elite Trainer Box Mega Gardevoir", "https://www.tcgplayer.com/product/644279", 80.00m, 0.00m);
            yield return new TestCaseData("196214114845", "648394", "Mega Evolution Elite Trainer Box Mega Lucario", "https://www.tcgplayer.com/product/648394", 80.00m, 0.00m);
            yield return new TestCaseData("196214132221", "649392", "Mega Heroes Mini Tin Display", "https://www.tcgplayer.com/product/649392", 200.00m, 0.00m);
            yield return new TestCaseData("196214134553", "656487", "Mega Lucario ex Premium Figure Collection", "https://www.tcgplayer.com/product/656487", 35.00m, 0.00m);
            yield return new TestCaseData("820650854163", "512813", "Paradox Rift Elite Trainer Box Iron Valiant", "https://www.tcgplayer.com/product/512813", 80.00m, 0.00m);
            yield return new TestCaseData("820650854163", "512815", "Paradox Rift Elite Trainer Box Roaring Moon", "https://www.tcgplayer.com/product/512815", 80.00m, 0.00m);
            yield return new TestCaseData("196214136748", "706309", "Perfect Order 3 Pack Blister", "https://www.tcgplayer.com/product/706309", 17.00m, 0.00m);
            yield return new TestCaseData("196214150461", "672394", "Perfect Order Booster Box", "https://www.tcgplayer.com/product/672394", 163.00m, 0.00m);
            yield return new TestCaseData("196214150478", "672396", "Perfect Order Booster Bundle", "https://www.tcgplayer.com/product/672396", 28.00m, 0.00m);
            yield return new TestCaseData("196214152038", "672401", "Perfect Order Elite Trainer Box", "https://www.tcgplayer.com/product/672401", 55.00m, 0.00m);
            yield return new TestCaseData("196214143371", "654160", "Phantasmal Flames Booster Bundle", "https://www.tcgplayer.com/product/654160", 70.00m, 0.00m);
            yield return new TestCaseData("196214124813", "654136", "Phantasmal Flames Elite Trainer Box", "https://www.tcgplayer.com/product/654136", 125.00m, 0.00m);
            yield return new TestCaseData("196214157514", "692939", "Pitch Black Booster Box", "https://www.tcgplayer.com/product/692939", 163.00m, 0.00m);
            yield return new TestCaseData("196214157422", "692947", "Pitch Black Elite Trainer Box", "https://www.tcgplayer.com/product/692947", 55.00m, 0.00m);
            yield return new TestCaseData("196214112766", "650799", "Prismatic Evolutions Premium Figure Collection", "https://www.tcgplayer.com/product/650799", 190.00m, 0.00m);
            yield return new TestCaseData("820650423123", "565606", "Surging Sparks Booster Box", "https://www.tcgplayer.com/product/565606", 280.00m, 0.00m);
            yield return new TestCaseData("820650859526", "565630", "Surging Sparks Elite Trainer Box", "https://www.tcgplayer.com/product/565630", 85.00m, 0.00m);
            yield return new TestCaseData("196214129924", "656997", "Team Rocket’s Moltres ex Ultra-Premium Collection", "https://www.tcgplayer.com/product/656997", 285.00m, 0.00m);

        }
            
    }
}

