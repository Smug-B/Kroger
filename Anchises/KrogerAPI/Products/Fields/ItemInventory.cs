using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct ItemInventory : ISerializable, IJsonParsable
{
    public string StockLevel { get; private set; }
    
    public ItemInventory() { }
    
    public ItemInventory(string stockLevel)
    {
        StockLevel = stockLevel;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("StockLevel", StockLevel);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        StockLevel = ioDictionary.Get<string>("StockLevel");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject informationObject = node.AsObject();
        string averageRatings = informationObject.TryGetProperty<string>("stockLevel");
        return new ItemInventory(averageRatings);
    }
}