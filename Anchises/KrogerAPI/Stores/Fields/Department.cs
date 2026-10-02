using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores.Fields;

public class Department : ISerializable, IJsonParsable
{
    public string DepartmentId { get; private set; }
    
    public string Name { get; private set; }

    public string Phone { get; private set; }

    public DepartmentHours Hours { get; private set; }
    
    public Department() { }

    public Department(string id, string name, string phone, DepartmentHours hours)
    {
        DepartmentId = id;
        Name = name;
        Phone = phone;
        Hours = hours;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("DepartmentID", DepartmentId);
        output.Add("Name", Name);
        output.Add("Phone", Phone);
        output.Add("Hours", Hours);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        DepartmentId = ioDictionary.Get<string>("DepartmentID");
        Name = ioDictionary.Get<string>("Name");
        Phone = ioDictionary.Get<string>("Phone");
        Hours = ioDictionary.Get<DepartmentHours>("Hours");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject departmentObject = node.AsObject();
        if (departmentObject.Count < 2)
        {
            throw new Exception($"Attempted to deserialize a JSON node as {nameof(Department)}." +
                                $"Expected at least 2 objects, but got {node.AsObject().Count}.)");
        }

        string id = departmentObject.GetProperty<string>("departmentId");
        string name = departmentObject.GetProperty<string>("name");
        string phone = departmentObject.TryGetProperty<string>("phone");
        DepartmentHours hours = departmentObject.TryGetProperty<DepartmentHours>("hours");
        return new Department(id, name, phone, hours);
    }
}