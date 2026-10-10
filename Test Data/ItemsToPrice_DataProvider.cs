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
            yield return new TestCaseData("196214158801", "704143", "30th Celebration Elite Trainer Box", "https://www.tcgplayer.com/product/704143");            
            yield return new TestCaseData("196214158726", "704168", "30th Celebration Greninja ex Box", "https://www.tcgplayer.com/product/704168");
            yield return new TestCaseData("196214158757", "704153", "30th Celebration Poster Collection", "https://www.tcgplayer.com/product/704153");
            yield return new TestCaseData("196214158771", "704146", "30th Celebration Tech Sticker Collection", "https://www.tcgplayer.com/product/704146");
            yield return new TestCaseData("196214131781", "666906", "Ascended Heroes Collection - Erika (3-Tab)", "https://www.tcgplayer.com/product/666906");
            yield return new TestCaseData("196214132474", "668496", "Ascended Heroes Elite Trainer Box", "https://www.tcgplayer.com/product/668496");
            yield return new TestCaseData("196214129238", "630430", "Black Bolt Binder Collection", "https://www.tcgplayer.com/product/630430");
            yield return new TestCaseData("820650809385", "242807", "Celebrations Collection Dragapult Prime", "https://www.tcgplayer.com/product/242807");
            yield return new TestCaseData("820650809439", "242811", "Celebrations Elite Trainer Box", "https://www.tcgplayer.com/product/242811");
            yield return new TestCaseData("196214154155", "684458", "Chaos Rising 3-Pack Blister", "https://www.tcgplayer.com/product/684458");
            yield return new TestCaseData("196214154186", "684444", "Chaos Rising Booster Box", "https://www.tcgplayer.com/product/684444");
            yield return new TestCaseData("196214154162", "684456", "Chaos Rising Booster Bundle", "https://www.tcgplayer.com/product/684456");
            yield return new TestCaseData("196214154117", "684450", "Chaos Rising Elite Trainer Box", "https://www.tcgplayer.com/product/684450");
            yield return new TestCaseData("196214150522", "673436", "First Partner Illustration Collection Series 1", "https://www.tcgplayer.com/product/673436");
            yield return new TestCaseData("196214155978", "688712", "First Partner Illustration Collection Series 2", "https://www.tcgplayer.com/product/688712");
            yield return new TestCaseData("196214135079", "684106", "League Battle Deck Mega Lucario ex", "https://www.tcgplayer.com/product/684106");
            yield return new TestCaseData("196214147539", "671250", "Mega Charizard Tin", "https://www.tcgplayer.com/product/671250");
            yield return new TestCaseData("196214130685", "654213", "Mega Charizard X ex Ultra Premium Collection", "https://www.tcgplayer.com/product/654213");
            yield return new TestCaseData("196214114845", "644279", "Mega Evolution Elite Trainer Box Mega Gardevoir", "https://www.tcgplayer.com/product/644279");
            yield return new TestCaseData("196214114845", "648394", "Mega Evolution Elite Trainer Box Mega Lucario", "https://www.tcgplayer.com/product/648394");
            yield return new TestCaseData("196214132221", "649392", "Mega Heroes Mini Tin Display", "https://www.tcgplayer.com/product/649392");
            yield return new TestCaseData("196214134553", "656487", "Mega Lucario ex Premium Figure Collection", "https://www.tcgplayer.com/product/656487");
            yield return new TestCaseData("820650854163", "512813", "Paradox Rift Elite Trainer Box Iron Valiant", "https://www.tcgplayer.com/product/512813");
            yield return new TestCaseData("820650854163", "512815", "Paradox Rift Elite Trainer Box Roaring Moon", "https://www.tcgplayer.com/product/512815");
            yield return new TestCaseData("196214136748", "706309", "Perfect Order 3 Pack Blister", "https://www.tcgplayer.com/product/706309");
            yield return new TestCaseData("196214150461", "672394", "Perfect Order Booster Box", "https://www.tcgplayer.com/product/672394");
            yield return new TestCaseData("196214150478", "672396", "Perfect Order Booster Bundle", "https://www.tcgplayer.com/product/672396");
            yield return new TestCaseData("196214152038", "672401", "Perfect Order Elite Trainer Box", "https://www.tcgplayer.com/product/672401");
            yield return new TestCaseData("196214143371", "654160", "Phantasmal Flames Booster Bundle", "https://www.tcgplayer.com/product/654160");
            yield return new TestCaseData("196214124813", "654136", "Phantasmal Flames Elite Trainer Box", "https://www.tcgplayer.com/product/654136");
            yield return new TestCaseData("196214157514", "692939", "Pitch Black Booster Box", "https://www.tcgplayer.com/product/692939");
            yield return new TestCaseData("196214157422", "692947", "Pitch Black Elite Trainer Box", "https://www.tcgplayer.com/product/692947");
            yield return new TestCaseData("196214112766", "650799", "Prismatic Evolutions Premium Figure Collection", "https://www.tcgplayer.com/product/650799");
            yield return new TestCaseData("820650423123", "565606", "Surging Sparks Booster Box", "https://www.tcgplayer.com/product/565606");
            yield return new TestCaseData("820650859526", "565630", "Surging Sparks Elite Trainer Box", "https://www.tcgplayer.com/product/565630");
            yield return new TestCaseData("196214129924", "656997", "Team Rocket’s Moltres ex Ultra-Premium Collection", "https://www.tcgplayer.com/product/656997");


            // **************************   ************  Datos para MTG
            yield return new TestCaseData("195166290522", "648686", "Avatar: The Last Airbender - Commander's Bundle", "https://www.tcgplayer.com/product/648686");
            yield return new TestCaseData("195166290416", "648643", "Avatar: The Last Airbender - Play Booster Display", "https://www.tcgplayer.com/product/648643");
            yield return new TestCaseData("195166257112", "541235", "Bloomburrow - Play Booster Display", "https://www.tcgplayer.com/product/541235");
            yield return new TestCaseData("195166270920", "618890", "FINAL FANTASY - Play Booster Display", "https://www.tcgplayer.com/product/618890");
            yield return new TestCaseData("195166306292", "656340", "Lorwyn Eclipsed - Bundle", "https://www.tcgplayer.com/product/656340");
            yield return new TestCaseData("195166305356", "656322", "Lorwyn Eclipsed - Collector Booster Display", "https://www.tcgplayer.com/product/656322");
            yield return new TestCaseData("195166305325", "656312", "Lorwyn Eclipsed - Play Booster Display", "https://www.tcgplayer.com/product/656312");
            yield return new TestCaseData("195166313139", "675625", "Marvel Super Heroes - Draft Night", "https://www.tcgplayer.com/product/675625");
            yield return new TestCaseData("195166315423", "675617", "Marvel Super Heroes - Gift Bundle", "https://www.tcgplayer.com/product/675617");
            yield return new TestCaseData("195166313603", "675603", "Marvel Super Heroes - Play Booster Display", "https://www.tcgplayer.com/product/675603");
            yield return new TestCaseData("195166313009", "675634", "Marvel Super Heroes Commander Deck - Avengers Assemble", "https://www.tcgplayer.com/product/675634");
            yield return new TestCaseData("195166313009", "675631", "Marvel Super Heroes Commander Deck - Doom Prevails", "https://www.tcgplayer.com/product/675631");
            yield return new TestCaseData("195166313009", "675632", "Marvel Super Heroes Commander Deck - The Fantastic Four", "https://www.tcgplayer.com/product/675632");
            yield return new TestCaseData("195166313009", "675633", "Marvel Super Heroes Commander Deck - Wakanda Forever", "https://www.tcgplayer.com/product/675633");
            yield return new TestCaseData("195166313108", "675619", "Marvel Super Heroes Scene Box - Heroes United", "https://www.tcgplayer.com/product/675619");
            yield return new TestCaseData("195166313108", "675620", "Marvel Super Heroes Scene Box - Villains Unleashed", "https://www.tcgplayer.com/product/675620");
            yield return new TestCaseData("195166337265", "692989", "Reality Fracture Commander Deck - Multiverse Reforged", "https://www.tcgplayer.com/product/692989");
            yield return new TestCaseData("195166318073", "675561", "Secrets of Strixhaven - Bundle", "https://www.tcgplayer.com/product/675561");
            yield return new TestCaseData("195166316703", "675555", "Secrets of Strixhaven - Play Booster Display", "https://www.tcgplayer.com/product/675555");
            yield return new TestCaseData("195166318035", "675568", "Secrets of Strixhaven Commander Deck - Prismari Artistry", "https://www.tcgplayer.com/product/675568");
            yield return new TestCaseData("195166308371", "657863", "Teenage Mutant Ninja Turtles - Pizza Bundle", "https://www.tcgplayer.com/product/657863");
            yield return new TestCaseData("195166308036", "657849", "Teenage Mutant Ninja Turtles - Play Booster Display", "https://www.tcgplayer.com/product/657849");
            yield return new TestCaseData("195166308333", "657865", "Teenage Mutant Ninja Turtles Commander Deck - Turtle Power", "https://www.tcgplayer.com/product/657865");
            yield return new TestCaseData("195166321981", "692966", "The Hobbit - Bundle", "https://www.tcgplayer.com/product/692966");
            yield return new TestCaseData("195166321851", "692960", "The Hobbit - Play Booster Display", "https://www.tcgplayer.com/product/692960");
            yield return new TestCaseData("195166322070", "692968", "The Hobbit Scene Box - Crack the Plates", "https://www.tcgplayer.com/product/692968");
            yield return new TestCaseData("195166322070", "692969", "The Hobbit Scene Box - Treasures of Smaug", "https://www.tcgplayer.com/product/692969");



            // **************************   ************  Datos para One Piece
            yield return new TestCaseData("810158838482", "682789", "Double Pack Set Vol. 10 - Adventure on Kami's Island (OP15-EB04)", "https://www.tcgplayer.com/product/682789");
            yield return new TestCaseData("810199500348", "693364", "Double Pack Set Vol. 11 - The Time of Battle (OP16)", "https://www.tcgplayer.com/product/693364");
            yield return new TestCaseData("810158836433", "666578", "Double Pack Set Vol. 9 - The Azure Sea's Seven (OP14-EB04)", "https://www.tcgplayer.com/product/666578");
            yield return new TestCaseData("810059786073", "561689", "Double Pack Set Volume 5 - Two Legends (OP08)", "https://www.tcgplayer.com/product/561689");
            yield return new TestCaseData("810158837225", "666891", "Extra Booster: One Piece Heroines Edition Box - Extra Booster EB03", "https://www.tcgplayer.com/product/666891");


            
            // **************************   ************  Datos para Riftbound
            yield return new TestCaseData("810155273248", "635368", "Origins - Booster Display", "https://www.tcgplayer.com/product/635368");
            yield return new TestCaseData("810155273583", "661934", "Spiritforged - Booster Display", "https://www.tcgplayer.com/product/661934");
            yield return new TestCaseData("810155274351", "678162", "Unleashed - Vault Bundle", "https://www.tcgplayer.com/product/678162");
            yield return new TestCaseData("810155275785", "693380", "Vendetta - Booster Display", "https://www.tcgplayer.com/product/693380");
            yield return new TestCaseData("810155275839", "697971", "Vendetta - Showdown Decks: Zed vs Shen", "https://www.tcgplayer.com/product/697971");            
        }
    }
}

