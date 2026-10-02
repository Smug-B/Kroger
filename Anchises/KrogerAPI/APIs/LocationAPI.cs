using System.Text.Json.Nodes;
using Anchises.KrogerAPI.Stores;
using Anchises.KrogerAPI.Stores.Fields;
using SmugBase.Extensions;

namespace Anchises.KrogerAPI.APIs;

public class LocationAPI
{
    public const string Version = "1.2.3";

    public static List<Store> ParseStore(JsonArray stores)
    {
        int numStores = stores.Count;
        List<Store> output = new List<Store>(numStores);
        for (int i = 0; i < numStores; i++)
        {
            JsonObject storeObject = stores[i].AsObject();
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
            output.Add(new Store(address, chain, phone, departments, geolocation, hours, locationId, storeNumber, divisionNumber, name));
        }
        return output;
    }
}