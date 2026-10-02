using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores.Fields;

public class Hours : ISerializable, IJsonParsable
{
    public bool Open24;

    public string GMTOffset;

    public string TimeZone;

    public Day Sunday;
    
    public Day Monday;
    
    public Day Tuesday;
    
    public Day Wednesday;
    
    public Day Thursday;
    
    public Day Friday;
    
    public Day Saturday;
    
    public Hours() { }

    public Hours(bool open24, string gmtOffset, string timeZone, Day sunday, Day monday, Day tuesday, Day wednesday, Day thursday, Day friday, Day saturday)
    {
        Open24 = open24;
        GMTOffset = gmtOffset;
        TimeZone = timeZone;
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
        output.Add("GMTOffset", GMTOffset);
        output.Add("TimeZone", TimeZone);
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
        GMTOffset = ioDictionary.Get<String>("GMTOffset");
        TimeZone = ioDictionary.Get<String>("TimeZone");
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
        bool open24 = hoursObject.HasProperty("open24")
            ? hoursObject.TryGetProperty<bool>("open24")
            : hoursObject.TryGetProperty<bool>("Open24");         
        string gmtOffset = hoursObject.TryGetProperty<string>("gmtOffset");
        string timeZone = hoursObject.TryGetProperty<string>("timezone");
        Day sunday = hoursObject.TryGetProperty<Day>("sunday");
        Day monday = hoursObject.TryGetProperty<Day>("monday");
        Day tuesday = hoursObject.TryGetProperty<Day>("tuesday");
        Day wednesday = hoursObject.TryGetProperty<Day>("wednesday");
        Day thursday = hoursObject.TryGetProperty<Day>("thursday");
        Day friday = hoursObject.TryGetProperty<Day>("friday");
        Day saturday = hoursObject.TryGetProperty<Day>("saturday");
        return new Hours(open24, gmtOffset, timeZone, sunday, monday, tuesday, wednesday, thursday, friday, saturday);
    }
}