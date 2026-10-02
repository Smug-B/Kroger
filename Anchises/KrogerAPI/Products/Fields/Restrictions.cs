using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct Restrictions : ISerializable, IJsonParsable
{
    public int MaxOrderQuantity { get; private set; }
    
    public int MinOrderQuantity { get; private set; }
    
    public IList<string> PostalCodes { get; private set; }
    
    public bool Shippable { get; private set; }
    
    public IList<string> StateCodes { get; private set; }
    
    public Restrictions() { }

    public Restrictions(int maxOrderQuantity, int minOrderQuantity, IList<string> postalCodes, bool shippable, IList<string> stateCodes)
    {
        MaxOrderQuantity = maxOrderQuantity;
        MinOrderQuantity = minOrderQuantity;
        PostalCodes = postalCodes;
        Shippable = shippable;
        StateCodes = stateCodes;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("MaxOrderQuantity", MaxOrderQuantity);
        output.Add("MinOrderQuantity", MinOrderQuantity);
        output.Add("PostalCodes", string.Join(" ", PostalCodes));
        output.Add("Shippable", Shippable);
        output.Add("StateCodes", string.Join(" ", StateCodes));
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        MaxOrderQuantity = ioDictionary.Get<int>("MaxOrderQuantity");
        MinOrderQuantity = ioDictionary.Get<int>("MinOrderQuantity");
        PostalCodes = ioDictionary.Get<string>("PostalCodes").Split(' ').ToList();
        Shippable = ioDictionary.Get<bool>("Shippable");
        StateCodes = ioDictionary.Get<string>("StateCodes").Split(' ').ToList();
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject restrictionObject = node.AsObject();
        int maxOrderQuantity = restrictionObject.TryGetProperty<int>("maximumOrderQuantity");
        int minOrderQuantity = restrictionObject.TryGetProperty<int>("minimumOrderQuantity");
        IList<string> postalCodes = restrictionObject.TryGetPropertyList<string>("postalCode");
        bool shippable = restrictionObject.TryGetProperty<bool>("shippable");
        IList<string> stateCodes = restrictionObject.TryGetPropertyList<string>("stateCodes");
        return new Restrictions(maxOrderQuantity, minOrderQuantity, postalCodes, shippable, stateCodes);
    }
}