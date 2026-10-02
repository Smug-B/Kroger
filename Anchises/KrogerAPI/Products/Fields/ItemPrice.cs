using System.Text.Json.Nodes;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products.Fields;

public struct ItemPrice : ISerializable, IJsonParsable
{
    public float Regular { get; private set; }
    
    public float Promotion { get; private set; }
    
    public float RegularPerUnitEstimate { get; private set; }
    
    public float PromotionPerUnitEstimate { get; private set; }
    
    public PriceDate ExpirationDate { get; private set; }
    
    public PriceDate EffectiveDate { get; private set; }
    
    public ItemPrice() { }
    
    public ItemPrice(float regular, 
        float promotion, 
        float regularPerUnitEstimate, 
        float promotionPerUnitEstimate, 
        PriceDate expirationDate, 
        PriceDate effectiveDate)
    {
        Regular = regular;
        Promotion = promotion;
        RegularPerUnitEstimate = regularPerUnitEstimate;
        PromotionPerUnitEstimate = promotionPerUnitEstimate;
        ExpirationDate = expirationDate;
        EffectiveDate = effectiveDate;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("Regular", Regular);
        output.Add("Promotion", Promotion);
        output.Add("RegularPerUnitEstimate", RegularPerUnitEstimate);
        output.Add("PromotionPerUnitEstimate", PromotionPerUnitEstimate);
        output.Add("ExpirationDate", ExpirationDate);
        output.Add("EffectiveDate", EffectiveDate);
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        Regular = ioDictionary.Get<float>("Regular");
        Promotion = ioDictionary.Get<float>("Promotion");
        RegularPerUnitEstimate = ioDictionary.Get<float>("RegularPerUnitEstimate");
        PromotionPerUnitEstimate = ioDictionary.Get<float>("PromotionPerUnitEstimate");
        ExpirationDate = ioDictionary.Get<PriceDate>("ExpirationDate");
        EffectiveDate = ioDictionary.Get<PriceDate>("EffectiveDate");
    }

    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject priceObject = node.AsObject();
        float regular = priceObject.TryGetProperty<float>("regular");
        float promotion = priceObject.TryGetProperty<float>("promo");
        float regularPerUnitEstimate = priceObject.TryGetProperty<float>("regularPerUnitEstimate");
        float promotionPerUnitEstimate = priceObject.TryGetProperty<float>("promoPerUnitEstimate");
        PriceDate expirationDate = priceObject.TryGetProperty<PriceDate>("expirationDate");
        PriceDate effectiveDate = priceObject.TryGetProperty<PriceDate>("effectiveDate");
        return new ItemPrice(regular, promotion, regularPerUnitEstimate, promotionPerUnitEstimate, expirationDate, effectiveDate);
    }
}