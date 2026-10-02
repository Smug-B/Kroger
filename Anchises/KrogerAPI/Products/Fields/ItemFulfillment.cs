using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct ItemFulfillment : ISerializable, IJsonParsable
{
    public bool Curbside { get; private set; }
    
    public bool Delivery { get; private set; }
    
    public bool InStore { get; private set; }
    
    public bool ShipToHome { get; private set; }
    
    public ItemFulfillment() { }
    
    public ItemFulfillment(bool curbside, bool delivery, bool inStore, bool shipToHome)
    {
        Curbside = curbside;
        Delivery = delivery;
        InStore = inStore;
        ShipToHome = shipToHome;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Curbside", Curbside);
        output.Add("Delivery", Delivery);
        output.Add("InStore", InStore);
        output.Add("ShipToHome", ShipToHome);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Curbside = ioDictionary.Get<bool>("Curbside");
        Delivery = ioDictionary.Get<bool>("Delivery");
        InStore = ioDictionary.Get<bool>("InStore");
        ShipToHome = ioDictionary.Get<bool>("ShipToHome");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject fulfillmentObject = node.AsObject();
        bool curbside = fulfillmentObject.TryGetProperty<bool>("curbside");
        bool delivery = fulfillmentObject.TryGetProperty<bool>("delivery");
        bool inStore = fulfillmentObject.TryGetProperty<bool>("instore");
        bool shipToHome = fulfillmentObject.TryGetProperty<bool>("shiptohome");
        return new ItemFulfillment(curbside, delivery, inStore, shipToHome);
    }
}