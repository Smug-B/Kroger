using System.IO.Compression;
using Anchises.KrogerAPI.APIs;
using Anchises.KrogerAPI.Products;
using SmugBase.Loading;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores;

public class StoreManager : IAutoloadable
{
    public static StoreManager Instance { get; private set; }
    
    public static string CacheFilePath { get; } = Path.Combine(Program.MainPath, "Cache", "Stores.cache");
    
    public static string ProductCacheFilePath { get; } = Path.Combine(Program.MainPath, "Cache", "Products");
    
    public List<Store> Stores { get; private set; }
    
    public Dictionary<string, Store> StoresById { get; private set; } = new Dictionary<string, Store>();
    
    public Dictionary<string, ProductManager> StoreProducts { get; private set; } = new Dictionary<string, ProductManager>();
    
    public void IAutoloadable_Load(IAutoloadable createdObject)
    {
        Instance = this;
    }
    
    public void IAutoloadable_Unload() { }

    public void PopulateFromUpstream(KrogerToken krogerToken)
    {
        Stores = Task.Run(async () => await KrogerApiClient.ApiClient.FetchLocations(krogerToken)).Result;
        foreach (Store store in Stores)
        {
            StoresById[store.LocationID] = store;
            StoreProducts.Add(store.LocationID, new ProductManager(store.LocationID));
        }
    }

    public void PopulateProductFromUpstream(KrogerToken krogerToken, string terms, int count)
    {
        foreach (Store store in Stores)
        {
            ProductManager productManager = StoreProducts[store.LocationID];
            productManager.PopulateFromUpstream(krogerToken, terms, count);
        }
    }
    
    public void PopulateFromCache()
    {
        if (!File.Exists(CacheFilePath))
        {
            return;
        }
        
        IODictionary cacheDictionary = IODictionary.DecompressFrom(CacheFilePath);
        foreach (string key in cacheDictionary.GetKeys())
        {
            Store store = cacheDictionary.Get<Store>(key);
            Stores.Add(store);
            StoresById[store.LocationID] = store;
        }
    }

    public void PopulateProductsFromCache()
    {
        string[] productCachesNames = Directory.GetFiles(ProductCacheFilePath);
        foreach (string productCacheName in productCachesNames)
        {
            IODictionary productCache = IODictionary.DecompressFrom(productCacheName);
            ProductManager productManager = new ProductManager();
            productManager.Load(productCache);
            StoreProducts[productManager.StoreId] =  productManager;
        }
    }

    public void Cache()
    {
        IODictionary cache = new IODictionary();
        foreach (Store store in Stores)
        {
            cache.Add(store.LocationID, store);
        }
        cache.CompressTo(CacheFilePath, CompressionLevel.Optimal);
    }

    public void CacheProducts()
    {
        foreach (ProductManager productManager in StoreProducts.Values)
        {
            IODictionary productCache = productManager.Save();
            productCache.CompressTo(productManager.CacheFilePath, CompressionLevel.Optimal);
        }
    }
}