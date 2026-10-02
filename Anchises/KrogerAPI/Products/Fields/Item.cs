using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct Item : ISerializable, IJsonParsable
{
    public string ItemId { get; private set; }

    public ItemInventory Inventory { get; private set; }
    
    public bool Favourite { get; private set; }
    
    public ItemFulfillment Fulfillment { get; private set; }
    
    public ItemPrice Price { get; private set; }
    
    public ItemPrice NationalPrice { get; private set; }
    
    public string Size { get; private set; }
    
    public string SoldBy { get; private set; }
    
    public Item() { }
    
    public Item(string itemId, 
        ItemInventory inventory, 
        bool favourite, 
        ItemFulfillment fulfillment,
        ItemPrice price, 
        ItemPrice nationalPrice, 
        string size, 
        string soldBy)
    {
        ItemId = itemId;
        Inventory = inventory;
        Favourite = favourite;
        Fulfillment = fulfillment;
        Price = price;
        NationalPrice = nationalPrice;
        Size = size;
        SoldBy = soldBy;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("ItemId", ItemId);
        output.Add("Inventory", Inventory);
        output.Add("Favourite", Favourite);
        output.Add("Fulfillment", Fulfillment);
        output.Add("Price", Price);
        output.Add("NationalPrice", NationalPrice);
        output.Add("Size", Size);
        output.Add("SoldBy", SoldBy);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        ItemId = ioDictionary.Get<string>("ItemId");
        Inventory = ioDictionary.Get<ItemInventory>("Inventory");
        Favourite = ioDictionary.Get<bool>("Favourite");
        Fulfillment = ioDictionary.Get<ItemFulfillment>("Fulfillment");
        Price = ioDictionary.Get<ItemPrice>("Price");
        NationalPrice = ioDictionary.Get<ItemPrice>("NationalPrice");
        Size = ioDictionary.Get<string>("Size");
        SoldBy = ioDictionary.Get<string>("SoldBy");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject itemObject = node.AsObject();
        string itemId = itemObject.TryGetProperty<string>("itemId");
        ItemInventory inventory = itemObject.TryGetProperty<ItemInventory>("inventory");
        bool favourite = itemObject.TryGetProperty<bool>("favourite");
        ItemFulfillment fulfillment = itemObject.TryGetProperty<ItemFulfillment>("fulfillment");
        ItemPrice price = itemObject.TryGetProperty<ItemPrice>("price");
        ItemPrice nationalPrice = itemObject.TryGetProperty<ItemPrice>("nationalPrice");
        string size = itemObject.TryGetProperty<string>("size");
        string soldBy = itemObject.TryGetProperty<string>("soldBy");
        return new Item(itemId, inventory, favourite, fulfillment, price, nationalPrice, size, soldBy);
    }
}