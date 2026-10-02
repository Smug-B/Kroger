using System.Text.Json;
using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores.Fields;

public struct Day : ISerializable, IJsonParsable
{
    public string Open { get; private set; }
        
    public string Close { get; private set; }

    public bool Open24 { get; private set; }
    
    public Day() { }

    public Day(string open, string close, bool open24)
    {
        Open = open;
        Close = close;
        Open24 = open24;
    }
        
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Open", Open);
        output.Add("Close", Close);
        output.Add("Open24", Open24);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Open = ioDictionary.Get<String>("Open");
        Close = ioDictionary.Get<String>("Close");
        Open24 = ioDictionary.Get<Boolean>("Open24");
    }
    
    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject dayObject = node.AsObject();
        if (dayObject.Count != 3)
        {
            throw new Exception($"Attempted to deserialize a JSON node as {nameof(Day)}." +
                                $"Expected 3 objects, but got {node.AsObject().Count}.)");
        }

        bool open24 = dayObject.GetProperty<bool>("open24");
        string open = dayObject.GetProperty<string>("open");
        JsonValueKind closeValueKind = dayObject.GetPropertyNode("close").GetValueKind();
        if (closeValueKind == JsonValueKind.Number)
        {
            int close = dayObject.GetProperty<int>("close");
            string closeHour = (close / 60).ToString("D2");
            string closeMinute = (close % 60).ToString("D2");
            return new Day(open, $"{closeHour}:{closeMinute}", open24);
        }
        else
        {
            string close = dayObject.GetProperty<string>("close");
            return new Day(open, close, open24);
        }
    }
}