using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.APIs;

public class KrogerToken : IJsonParsable
{
    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public int Expiration { get; set; }

    public KrogerToken() { }

    public KrogerToken(string accessToken,
        string tokenType,
        int expiration)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        Expiration = expiration;
    }

    public KrogerToken(JsonNode tokenJson)
    {
        JsonObject tokenObject = tokenJson.AsObject();
        AccessToken = tokenObject.TryGetProperty<string>("access_token");
        TokenType = tokenObject.TryGetProperty<string>("token_type");
        Expiration = tokenObject.TryGetProperty<int>("expires_in");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject tokenObject = node.AsObject();
        string accessToken = tokenObject.TryGetProperty<string>("access_token");
        string tokenType = tokenObject.TryGetProperty<string>("token_type");
        int expiration = tokenObject.TryGetProperty<int>("expires_in");
        return new KrogerToken(accessToken, tokenType, expiration);
    }
}