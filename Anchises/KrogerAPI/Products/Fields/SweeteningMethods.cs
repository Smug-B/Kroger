using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct SweeteningMethods : ISerializable, IJsonParsable
{
    public string Code { get; private set; }

    public string Name { get; private set; }
    
    public SweeteningMethods() { }

    public SweeteningMethods(string code, string name)
    {
        Code = code;
        Name = name;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Code", Code);
        output.Add("Name", Name);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Code = ioDictionary.Get<string>("Code");
        Name = ioDictionary.Get<string>("Name");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject sweeteningObject = node is JsonArray ? node[0].AsObject() : node.AsObject();
        string code = sweeteningObject.TryGetProperty<string>("code");
        string name = sweeteningObject.TryGetProperty<string>("name");
        return new SweeteningMethods(code, name);
    }
}