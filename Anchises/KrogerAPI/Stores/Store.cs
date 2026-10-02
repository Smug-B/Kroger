using System.Text.Json.Nodes;
using Anchises.KrogerAPI.Stores.Fields;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Stores;

public class Store : ISerializable, IJsonParsable
{
    public Address Address;

    public string Chain;

    public string Phone;
    
    public IList<Department> Departments;
    
    public Geolocation Geolocation;

    public Hours Hours;
    
    public string LocationID;

    public string StoreNumber;

    public string DivisionNumber;

    public string Name;

    public Store() { }

    public Store(Address address,
        string chain,
        string phone,
        IList<Department> departments,
        Geolocation geolocation,
        Hours hours,
        string locationId,
        string storeNumber,
        string divisionNumber,
        string name)
    {
        Address = address;
        Chain = chain;
        Phone = phone;
        Departments = departments;
        Geolocation = geolocation;
        Hours = hours;
        LocationID = locationId;
        StoreNumber = storeNumber;
        DivisionNumber = divisionNumber;
        Name = name;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Address", Address);
        output.Add("Chain", Chain);
        output.Add("Phone", Phone);
        output.Add("Geolocation", Geolocation);
        output.Add("Hours", Hours);
        output.Add("LocationID", LocationID);
        output.Add("StoreNumber", StoreNumber);
        output.Add("DivisionNumber", DivisionNumber);
        output.Add("Name", Name);

        if (Departments != null)
        {
            IODictionary departments = new IODictionary();
            departments.Add("Count", Departments.Count);
            for (int i = 0; i < Departments.Count; i++)
            {
                output.Add(i.ToString(), Departments[i]);
            }
            output.Add("Departments", departments);
        }
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Address = ioDictionary.Get<Address>("Address");
        Chain = ioDictionary.Get<string>("Chain");
        Phone = ioDictionary.Get<string>("Phone");
        Hours = ioDictionary.Get<Hours>("Hours");
        LocationID = ioDictionary.Get<string>("LocationID");
        StoreNumber = ioDictionary.Get<string>("StoreNumber");
        DivisionNumber = ioDictionary.Get<string>("DivisionNumber");
        Name = ioDictionary.Get<string>("Name");

        if (ioDictionary.ContainsKey("Departments"))
        {
            IODictionary departments = ioDictionary.Get<IODictionary>("Departments");
            int departmentCount = departments.Get<int>("Count");
            Departments = new List<Department>(departmentCount);
            for (int i = 0; i < departmentCount; i++)
            {
                Department department = departments.Get<Department>(i.ToString());
                Departments.Add(department);
            }
        }
    }
    
    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject storeObject = node.AsObject();
        Address address = storeObject.TryGetProperty<Address>("address");
        string chain = storeObject.TryGetProperty<string>("chain");
        string phone = storeObject.TryGetProperty<string>("phone");
        IList<Department> departments = storeObject.TryGetPropertyList<Department>("departments");
        Geolocation geolocation = storeObject.TryGetProperty<Geolocation>("geolocation");
        Hours hours = storeObject.TryGetProperty<Hours>("hours");
        string locationId = storeObject.TryGetProperty<string>("locationId");
        string storeNumber = storeObject.TryGetProperty<string>("storeNumber");
        string divisionNumber = storeObject.TryGetProperty<string>("divisionNumber");
        string name = storeObject.TryGetProperty<string>("name");
        return new Store(address, chain, phone, departments, geolocation, hours, locationId, storeNumber, divisionNumber, name);
    }
}