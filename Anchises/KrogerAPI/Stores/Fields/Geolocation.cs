using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores.Fields;

public struct Geolocation : ISerializable, IJsonParsable
{
    private int InternalLatitude;
    
    private int InternalLongitude;

    private const double ScaleFactor = 1e5;

    public double Latitude
    {
        get => InternalLatitude / ScaleFactor; 
        private set => InternalLatitude = (int)Math.Round(value * ScaleFactor);
    }
    
    public double Longitude
    {
        get => InternalLongitude / ScaleFactor; 
        private set => InternalLongitude = (int)Math.Round(value * ScaleFactor);
    }
    
    public Geolocation() { }

    public Geolocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Latitude", InternalLatitude);
        output.Add("Longitude", InternalLongitude);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        InternalLatitude = ioDictionary.Get<int>("Latitude");
        InternalLongitude = ioDictionary.Get<int>("Longitude");
    }
    
    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject geolocationObject = node.AsObject();
        if (geolocationObject.Count != 3)
        {
            throw new Exception($"Attempted to deserialize a JSON node as {nameof(Geolocation)}." +
                                $"Expected 3 objects, but got {node.AsObject().Count}.)");
        }
        
        double latitude = geolocationObject.GetProperty<double>("latitude");
        double longitude = geolocationObject.GetProperty<double>("longitude");
        return new Geolocation(latitude, longitude);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(InternalLatitude, InternalLongitude);
    }
}