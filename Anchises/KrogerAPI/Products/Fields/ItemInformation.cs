using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct ItemInformation : ISerializable, IJsonParsable
{
    public float Depth { get; private set; }

    public float Width { get; private set; }
    
    public float Height { get; private set; }

    public string GrossWeight { get; private set; }
    
    public string NetWeight { get; private set; }

    public string AverageUnitWeight { get; private set; }
    
    public ItemInformation() { }
    
    public ItemInformation(float depth, float width, float height, string grossWeight, string netWeight, string averageUnitWeight)
    {
        Depth = depth;
        Width = width;
        Height = height;
        GrossWeight = grossWeight;
        NetWeight = netWeight;
        AverageUnitWeight = averageUnitWeight;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Depth", Depth);
        output.Add("Width", Width);
        output.Add("Height", Height);
        output.Add("GrossWeight", GrossWeight);
        output.Add("NetWeight", NetWeight);
        output.Add("AverageUnitWeight", AverageUnitWeight);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Depth = ioDictionary.Get<int>("Depth");
        Width = ioDictionary.Get<int>("Width");
        Height = ioDictionary.Get<int>("Height");
        GrossWeight = ioDictionary.Get<string>("GrossWeight");
        NetWeight = ioDictionary.Get<string>("NetWeight");
        AverageUnitWeight = ioDictionary.Get<string>("AverageUnitWeight");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject informationObject = node.AsObject();
        string depth = informationObject.TryGetProperty<string>("depth");
        string width = informationObject.TryGetProperty<string>("width");
        string height = informationObject.TryGetProperty<string>("height");
        string grossWeight = informationObject.TryGetProperty<string>("grossWeight");
        string netWeight = informationObject.TryGetProperty<string>("netWeight");
        string averageWeightPerUnit = informationObject.TryGetProperty<string>("averageWeightPerUnit");
        return new ItemInformation(float.Parse(depth), float.Parse(width), float.Parse(height), grossWeight, netWeight, averageWeightPerUnit);
    }
}