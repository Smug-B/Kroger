using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct Temperature : ISerializable, IJsonParsable
{
    public string Indicator { get; private set; }

    public bool HeatSensitive { get; private set; }
    
    public Temperature() { }
    
    public Temperature(string indicator, bool heatSensitive)
    {
        Indicator = indicator;
        HeatSensitive = heatSensitive;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Indicator", Indicator);
        output.Add("HeatSensitive", HeatSensitive);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Indicator = ioDictionary.Get<string>("Indicator");
        HeatSensitive = ioDictionary.Get<bool>("HeatSensitive");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject temperatureObject = node.AsObject();
        string indicator = temperatureObject.TryGetProperty<string>("indicator");
        bool heatSensitive = temperatureObject.TryGetProperty<bool>("heatSensitive");
        return new Temperature(indicator, heatSensitive);
    }
}