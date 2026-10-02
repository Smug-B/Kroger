using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores.Fields;

public class DepartmentHours : ISerializable, IJsonParsable
{
    public bool Open24;
    
    public Day Sunday;
    
    public Day Monday;
    
    public Day Tuesday;
    
    public Day Wednesday;
    
    public Day Thursday;
    
    public Day Friday;
    
    public Day Saturday;
    
    public DepartmentHours() { }

    public DepartmentHours(bool open24, Day sunday, Day monday, Day tuesday, Day wednesday, Day thursday, Day friday, Day saturday)
    {
        Open24 = open24;
        Sunday = sunday;
        Monday = monday;
        Tuesday = tuesday;
        Wednesday = wednesday;
        Thursday = thursday;
        Friday = friday;
        Saturday = saturday;
    }   
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Open24", Open24);
        output.Add("Sunday", Sunday);
        output.Add("Monday", Monday);
        output.Add("Tuesday", Tuesday);
        output.Add("Wednesday", Wednesday);
        output.Add("Thursday", Thursday);
        output.Add("Friday", Friday);
        output.Add("Saturday", Saturday);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Open24 = ioDictionary.Get<Boolean>("Open24");
        Sunday = ioDictionary.Get<Day>("Sunday");
        Monday = ioDictionary.Get<Day>("Monday");
        Tuesday = ioDictionary.Get<Day>("Tuesday");
        Wednesday = ioDictionary.Get<Day>("Wednesday");
        Thursday = ioDictionary.Get<Day>("Thursday");
        Friday = ioDictionary.Get<Day>("Friday");
        Saturday = ioDictionary.Get<Day>("Saturday");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject hoursObject = node.AsObject();
        if (hoursObject.Count != 8)
        {
            throw new Exception($"Attempted to deserialize a JSON node as {nameof(DepartmentHours)}." +
                                $"Expected 8 objects, but got {node.AsObject().Count}.)");
        }

        bool open24 = hoursObject.HasProperty("open24")
            ? hoursObject.TryGetProperty<bool>("open24")
            : hoursObject.TryGetProperty<bool>("Open24"); 
        Day sunday = hoursObject.GetProperty<Day>("sunday");
        Day monday = hoursObject.GetProperty<Day>("monday");
        Day tuesday = hoursObject.GetProperty<Day>("tuesday");
        Day wednesday = hoursObject.GetProperty<Day>("wednesday");
        Day thursday = hoursObject.GetProperty<Day>("thursday");
        Day friday = hoursObject.GetProperty<Day>("friday");
        Day saturday = hoursObject.GetProperty<Day>("saturday");
        return new DepartmentHours(open24, sunday, monday, tuesday, wednesday, thursday, friday, saturday);
    }
}