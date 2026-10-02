using System.IO.Compression;
using System.Text.Json.Nodes;
using Anchises.KrogerAPI.APIs;
using SmugBase.Loading;
using SmugBase.Logging;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products;

public class ProductManager : ISerializable
{
    public string StoreId { get; private set; }
    
    public string CacheFilePath => Path.Combine(Program.MainPath, "Cache", "Products", StoreId + ".cache");

    public List<Product> Products { get; private set; } = new List<Product>();
    
    public Dictionary<string, Product> ProductsById { get; private set; } = new Dictionary<string, Product>();
    
    public ProductManager() { }
    
    public ProductManager(string storeId)
    {
        StoreId = storeId;
    }
    
    public void PopulateFromUpstream(KrogerToken krogerToken, string terms, int count)
    {
        try
        {
            JsonArray products = Task.Run(async () => await KrogerApiClient.ApiClient.FetchProducts(krogerToken, terms, StoreId, count)).Result;
            int numProducts = products.Count;
            Product dummy = new Product();
            for (int i = 0; i < numProducts; i++)
            {
                JsonObject productObject = products[i].AsObject();
                Product product = dummy.Parse(productObject) as Product;
                Products.Add(product);
                ProductsById[product.ProductId] = product;
            }
        }
        catch (Exception e)
        {
            ContentManager.GetInstance<Logger>().Log(e, LogType.Error);
        }
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("StoreId", StoreId);
        
        IODictionary products = new IODictionary();
        products.Add("Count", Products.Count);
        for (int i = 0; i < Products.Count; i++)
        {
            products.Add(i.ToString(), Products[i]);
        }
        
        output.Add("Products", products);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        StoreId = ioDictionary.Get<string>("StoreId");
        IODictionary products = ioDictionary.Get<IODictionary>("Products");
        int productCount = products.Get<int>("Count");
        for (int i = 0; i < productCount; i++)
        {
            Products.Add(products.Get<Product>(i.ToString()));
        }
    }
}