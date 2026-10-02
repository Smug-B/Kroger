using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct Ratings : ISerializable, IJsonParsable
{
    public int AverageRatings { get; private set; }

    public int ReviewCount { get; private set; }
    
    public Ratings() { }
    
    public Ratings(int averageRatings, int reviewCount)
    {
        AverageRatings = averageRatings;
        ReviewCount = reviewCount;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("AverageRatings", AverageRatings);
        output.Add("ReviewCount", ReviewCount);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        AverageRatings = ioDictionary.Get<int>("AverageRatings");
        ReviewCount = ioDictionary.Get<int>("ReviewCount");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject itemInformationObject = node.AsObject();
        int averageRatings = itemInformationObject.TryGetProperty<int>("averageOverallRating");
        int reviewCount = itemInformationObject.TryGetProperty<int>("totalReviewCount");
        return new Ratings(averageRatings, reviewCount);
    }
}