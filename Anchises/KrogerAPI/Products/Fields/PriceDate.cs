using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct PriceDate : ISerializable, IJsonParsable
{
    public string Value { get; private set; }
    
    public string TimeZone { get; private set; }
    
    public PriceDate() { }
    
    public PriceDate(string value, string timeZone)
    {
        Value = value;
        TimeZone = timeZone;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Value", Value);
        output.Add("TimeZone", TimeZone);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Value = ioDictionary.Get<string>("Value");
        TimeZone = ioDictionary.Get<string>("TimeZone");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject priceDateObject = node.AsObject();
        string value = priceDateObject.TryGetProperty<string>("value");
        string timeZone = priceDateObject.TryGetProperty<string>("timezone");
        return new PriceDate(value, timeZone);
    }
}