using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Anchises.KrogerAPI.APIs;
using Anchises.KrogerAPI.Stores;
using SmugBase.Loading;
using SmugBase.Logging;

namespace Anchises.KrogerAPI;

public class KrogerApiClient : IAutoloadable
{
    // TODO: Anchises should be able to regularly fetch the status of ALL Kroger stores. This should happen every Sunday.
    // TODO: Anchises COULD be responsible for fetching additional information pertaining to the demographics in the community associated with a given Kroger store.
    // TODO: Anchises COULD be responsible for plotting this data, GIS style.
    
    public static KrogerApiClient ApiClient { get; private set; }
    
    public string? ClientId { get; private set; }
    
    public string? ClientSecret { get; private set; }
    
    public HttpClient? KrogerClient { get; private set; }
    
    public void IAutoloadable_Load(IAutoloadable createdObject)
    {
        string clientIdPath = Path.Combine(Program.MainPath, KrogerUri.IsProduction ? "ClientId.txt" : "TestClientId.txt");
        string clientSecretPath = Path.Combine(Program.MainPath, KrogerUri.IsProduction ? "ClientSecret.txt" : "TestClientSecret.txt");
        if (!File.Exists(clientIdPath) || !File.Exists(clientSecretPath))
        {
            throw new Exception("Could not find client information files at: " + clientIdPath + " and/or " +
                                clientSecretPath +
                                "\nThese files must only contain their respective information. They can be acquired from: " +
                                KrogerUri.DevPortal);
        }
        ClientId = File.ReadAllText(clientIdPath);
        ClientSecret = File.ReadAllText(clientSecretPath);
        
        HttpClientHandler handler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        KrogerClient = new HttpClient(handler);

        ApiClient = this;
    }

    public void IAutoloadable_Unload()
    {
        KrogerClient?.Dispose();
    }

    public string GetCredentials()
    {
        if (ClientId == null || ClientSecret == null)
        {
            throw new Exception("Cannot get credentials before loading has fully finished.");
        }
        return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{ClientId}:{ClientSecret}"));
    }
    
    public async Task<KrogerToken> GenerateToken()
    {
        if (ClientId == null || ClientSecret == null || KrogerClient == null)
        {
            throw new Exception("Cannot generate Kroger API token before loading has fully finished.");
        }

        string credentials = GetCredentials();
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, KrogerUri.Token);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        request.Content = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["scope"] = "product.compact"
            }
        );
        
        HttpResponseMessage response = await KrogerClient.SendAsync(request);
        string body = await response.Content.ReadAsStringAsync();
        ContentManager.GetInstance<Logger>().Log($"Attempted to fetch token with status code: {response.StatusCode}, body: {body}");
        return new KrogerToken(JsonNode.Parse(body));
    }

    public async Task<List<Store>> FetchLocations(KrogerToken krogerToken)
    {
        if (ClientId == null || ClientSecret == null || KrogerClient == null)
        {
            throw new Exception("Cannot fetch Kroger location data before loading has fully finished.");
        }
        
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, KrogerUri.Locations);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", krogerToken.AccessToken);

        HttpResponseMessage response = await KrogerClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        return LocationAPI.ParseStore(JsonNode.Parse(json)![0].AsArray());
    }

    public async Task<JsonArray> FetchProducts(KrogerToken krogerToken, string term, string storeId, int start = 1, int limit = 10)
    {
        if (ClientId == null || ClientSecret == null || KrogerClient == null)
        {
            throw new Exception("Cannot fetch Kroger product data before loading has fully finished.");
        }

        start = Math.Clamp(start, 1, 250);
        limit = Math.Clamp(limit, 1, 50);
        
        string productString = KrogerUri.Products + "?" +
                               $"filter.term={Uri.EscapeDataString(term)}&" +
                               $"filter.locationId={storeId}&" +
                               $"filter.start={start}&" +
                               $"filter.limit={limit}";
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, productString);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", krogerToken.AccessToken);

        HttpResponseMessage response = await KrogerClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        return JsonNode.Parse(json)![0].AsArray();
    }
}