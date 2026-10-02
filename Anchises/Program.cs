using Anchises.KrogerAPI;
using Anchises.KrogerAPI.APIs;
using Anchises.KrogerAPI.Stores;
using SmugBase.Loading;
using SmugBase.Utility;
using Logger = SmugBase.Logging.Logger;

namespace Anchises
{
    public class Program
    {
        public static Logger Logger { get; private set; }
        
        public static string MainPath { get; } = FileUtility.GetDirectory("Projects", "Kroger", "Anchises", "Data");

        public static string[] CoreItems =
        [
            "milk",
            "eggs",
            "butter",
            "cheese",
            "yogurt",
            "chicken",
            "beef",
            "steak",
            "pork",
            "fish",
            "shrimp",
            "turkey",
            "ham",
            "bread",
            "muffins",
            "tortillas",
            "rice",
            "pasta",
            "sauce",
            "oil",
            "beans",
            "flour",
            "salt",
            "sugar",
            "spices",
            "peanut butter",
            "jelly",
            "water",
            "soda",
            "coffee",
            "tea",
            "chips",
            "granola",
            "cereal",
            "protein",
            "frozen",
            "ice cream",
            "paper",
            "detergent",
            "soap",
            "shampoo",
            "wash",
            "conditioner",
            "toothpaste",
            "banana",
            "apple",
            "potato",
            "tomato",
            "orange",
            "spinach",
            "kale",
            "onions",
            "fruits",
            "vegetables"
        ];

        static Program()
        {
            Logger = new Logger("Main.log", MainPath);
        }
        
        public static void Main(string[] args)
        {
            LoadingHandler.ImplementLoading(Logger);
            
            KrogerToken krogerToken = Task.Run(async () => await KrogerApiClient.ApiClient.GenerateToken()).Result;
            StoreManager.Instance.PopulateFromUpstream(krogerToken);
            StoreManager.Instance.Cache();
            for (int i = 0; i < 3; i++)
            {
                StoreManager.Instance.PopulateProductFromUpstream(krogerToken, CoreItems[i], 10);
                StoreManager.Instance.CacheProducts();
            }
        }
    }
}