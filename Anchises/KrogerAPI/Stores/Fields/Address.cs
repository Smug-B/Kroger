using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores.Fields;

public struct Address : ISerializable, IJsonParsable
{
    public string AddressLine1 { get; private set; }

    public int BuildingNumber;

    public string StreetName;
    
    public string StreetExtension;
    
    public string AddressLine2 { get; private set; }

    public string City { get; private set; }

    public string County { get; private set; }

    public string State { get; private set; }

    public string Zip { get; private set; }
    
    public Address() { }
    
    public Address(string addressLine1, string addressLine2, string city, string county, string state, string zip)
    {
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        County = county;
        State = state;
        Zip = zip;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("AddressLine1", AddressLine1);
        output.Add("AddressLine2", AddressLine2);
        output.Add("City", City);
        output.Add("County", County);
        output.Add("State", State);
        output.Add("Zip", Zip);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        AddressLine1 = ioDictionary.Get<string>("AddressLine1");
        AddressLine2 = ioDictionary.Get<string>("AddressLine2");
        City = ioDictionary.Get<string>("City");
        County = ioDictionary.Get<string>("County");
        State = ioDictionary.Get<string>("State");
        Zip = ioDictionary.Get<string>("Zip");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject addressObject = node.AsObject();
        if (addressObject.Count != 5 && addressObject.Count != 6)
        {
            throw new Exception($"Attempted to deserialize a JSON node as {nameof(Address)}." +
                                $"Expected 6 objects, but got {node.AsObject().Count}.)");
        }

        string addressLine1 = addressObject.GetProperty<string>("addressLine1");
        string addressLine2 = "";
        try
        {
            addressObject.TryGetProperty<string>("addressLine2");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        string city = addressObject.GetProperty<string>("city");
        string county = addressObject.GetProperty<string>("county");
        string state = addressObject.GetProperty<string>("state");
        string zip = addressObject.GetProperty<string>("zipCode");
        return new Address(addressLine1, addressLine2, city, county, state, zip);
    }
}