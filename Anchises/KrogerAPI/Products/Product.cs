using System.Text.Json.Nodes;
using Anchises.KrogerAPI.Products.Fields;
using SmugBase.Extensions;
using SmugBase.Saving;

namespace Anchises.KrogerAPI.Products;

// We omit a lot of likely redundant data. 
// Currently, the omission list is as follows:
// - Aisle location
// - Allergens
// - Images
// - Nutrition Information
public class Product : ISerializable, IJsonParsable
{
    public string ProductId { get; private set; }
    
    public string ProductPageUri { get; private set; }
    
    public IList<string> AliasProductIds { get; private set; }
    
    public string Brand { get; private set; }
    
    public IList<string> Categories { get; private set; }
    
    public string CountryOrigin { get; private set; }
    
    public string Description { get; private set; }
    
    public bool Alcohol { get; private set; }
    
    public int AlcoholProof { get; private set; }
    
    public bool AgeRestricted { get; private set; }
    
    public bool SnapEligible { get; private set; }
    
    public IList<string> ManufacturerDeclarations { get; private set; }
    
    public SweeteningMethods SweeteningMethods { get; private set; }
    
    public bool PassoverCertified { get; private set; }
    
    public bool Hypoallergenic { get; private set; }
    
    public bool NonGmo { get; private set; }
    
    public string NonGmoClaim { get; private set; }
    
    public string OrganicClaim { get; private set; }
    
    public string ReceiptDescription { get; private set; }
    
    public string Warnings { get; private set; }
    
    public Restrictions Restrictions { get; private set; }
    
    public IList<Item> Items { get; private set; }
    
    public ItemInformation ItemInformation { get; private set; }
    
    public Temperature Temperature { get; private set; }
    
    public string Upc { get; private set; }
    
    public Ratings Ratings { get; private set; }
    
    public Product() { }

    public Product(string productId, 
        string productPageUri, 
        IList<string> aliasProductIds,
        string brand, 
        IList<string> categories, 
        string countryOrigin,
        string description,
        bool alcohol,
        int alcoholProof,
        bool ageRestricted,
        bool snapEligible,
        IList<string> manufacturerDeclarations,
        SweeteningMethods sweeteningMethods,
        bool passoverCertified,
        bool hypoallergenic,
        bool nonGmo,
        string nonGmoClaim,
        string organicClaim,
        string receiptDescription,
        string warnings,
        Restrictions restrictions,
        IList<Item> items,
        ItemInformation itemInformation,
        Temperature temperature,
        string upc,
        Ratings ratings)
    {
        ProductId = productId;
        ProductPageUri = productPageUri;
        AliasProductIds = aliasProductIds;
        Brand = brand;
        Categories = categories;
        CountryOrigin = countryOrigin;
        Description = description;
        Alcohol = alcohol;
        AlcoholProof = alcoholProof;
        AgeRestricted = ageRestricted;
        SnapEligible = snapEligible;
        ManufacturerDeclarations = manufacturerDeclarations;
        SweeteningMethods = sweeteningMethods;
        PassoverCertified = passoverCertified;
        Hypoallergenic = hypoallergenic;
        NonGmo = nonGmo;
        NonGmoClaim = nonGmoClaim;
        OrganicClaim = organicClaim;
        ReceiptDescription = receiptDescription;
        Warnings = warnings;
        Restrictions = restrictions;
        Items = items;
        ItemInformation = itemInformation;
        Temperature = temperature;
        Upc = upc;
        Ratings = ratings;
    }
    
    public IODictionary Save()
    {
        IODictionary output = new IODictionary();
        output.Add("ProductId", ProductId);
        output.Add("ProductPageUri", ProductPageUri);
        output.Add("AliasProductIds", string.Join(' ', AliasProductIds));
        output.Add("Brand", Brand);
        output.Add("Categories", string.Join(' ', Categories));
        output.Add("CountryOrigin", CountryOrigin);
        output.Add("Description", Description);
        output.Add("Alcohol", Alcohol);
        output.Add("AlcoholProof", AlcoholProof);
        output.Add("AgeRestricted", AgeRestricted);
        output.Add("SnapEligible", SnapEligible);
        output.Add("ManufacturerDeclarations", string.Join(' ', ManufacturerDeclarations));
        output.Add("SweeteningMethods", SweeteningMethods);
        output.Add("PassoverCertified", PassoverCertified);
        output.Add("Hypoallergenic", Hypoallergenic);
        output.Add("NonGmo", NonGmo);
        output.Add("NonGmoClaim", NonGmoClaim);
        output.Add("OrganicClaim", OrganicClaim);
        output.Add("ReceiptDescription", ReceiptDescription);
        output.Add("Warnings", Warnings);
        output.Add("Restrictions", Restrictions);
        output.Add("NonGmoClaim", NonGmoClaim);
        output.Add("ItemInformation", ItemInformation);
        output.Add("Temperature", Temperature);
        output.Add("Upc", Upc);
        output.Add("Ratings", Ratings);
        
        if (Items != null)
        {
            IODictionary items = new IODictionary();
            items.Add("Count", Items.Count);
            for (int i = 0; i < Items.Count; i++)
            {
                output.Add(i.ToString(), Items[i]);
            }
            output.Add("Items", items);
        }
        return output;
    }

    public void Load(IODictionary ioDictionary)
    {
        ProductId = ioDictionary.Get<string>("ProductId");
        ProductPageUri = ioDictionary.Get<string>("ProductPageUri");
        AliasProductIds = ioDictionary.Get<string>("AliasProductIds").Split(' ').ToList();
        Brand = ioDictionary.Get<string>("Brand");
        Categories = ioDictionary.Get<string>("Categories").Split(' ').ToList();
        CountryOrigin = ioDictionary.Get<string>("CountryOrigin");
        Description = ioDictionary.Get<string>("Description");
        Alcohol = ioDictionary.Get<bool>("Alcohol");
        AlcoholProof = ioDictionary.Get<int>("AlcoholProof");
        AgeRestricted = ioDictionary.Get<bool>("AgeRestricted");
        SnapEligible = ioDictionary.Get<bool>("SnapEligible");
        ManufacturerDeclarations = ioDictionary.Get<string>("ManufacturerDeclarations").Split(' ').ToList();
        SweeteningMethods = ioDictionary.Get<SweeteningMethods>("SweeteningMethods");
        PassoverCertified = ioDictionary.Get<bool>("PassoverCertified");
        Hypoallergenic = ioDictionary.Get<bool>("Hypoallergenic");
        NonGmo = ioDictionary.Get<bool>("NonGmo");
        NonGmoClaim = ioDictionary.Get<string>("NonGmoClaim");
        OrganicClaim = ioDictionary.Get<string>("OrganicClaim");
        ReceiptDescription = ioDictionary.Get<string>("ReceiptDescription");
        Warnings = ioDictionary.Get<string>("Warnings");
        Restrictions = ioDictionary.Get<Restrictions>("Restrictions");
        ItemInformation = ioDictionary.Get<ItemInformation>("ItemInformation");
        Temperature = ioDictionary.Get<Temperature>("Temperature");
        Upc = ioDictionary.Get<string>("Upc");
        Ratings = ioDictionary.Get<Ratings>("Ratings");

        if (ioDictionary.ContainsKey("Items"))
        {
            IODictionary items = ioDictionary.Get<IODictionary>("Items");
            int itemsCount = items.Get<int>("Count");
            Items = new List<Item>(itemsCount);
            for (int i = 0; i < itemsCount; i++)
            {
                Items.Add(items.Get<Item>(i.ToString()));
            }
        }
    }
    
    public IJsonParsable Parse(JsonNode node)
    {
        JsonObject productObject = node.AsObject();
        string productId = productObject.TryGetProperty<string>("productId");
        string productPageUri = productObject.TryGetProperty<string>("productPageURI");
        IList<string> aliasProductIds = productObject.TryGetPropertyList<string>("aliasProductIds");
        string brand = productObject.TryGetProperty<string>("brand");
        IList<string> categories = productObject.TryGetPropertyList<string>("categories");
        string countryOrigin = productObject.TryGetProperty<string>("countryOrigin");
        string description = productObject.TryGetProperty<string>("description");
        bool alcohol = productObject.TryGetProperty<bool>("alcohol");
        int alcoholProof = productObject.TryGetProperty<int>("alcoholProof");
        bool ageRestricted = productObject.TryGetProperty<bool>("ageRestriction");
        bool snapEligible = productObject.TryGetProperty<bool>("snapEligible");
        IList<string> manufacturerDeclarations = productObject.TryGetPropertyList<string>("manufacturerDeclarations");
        SweeteningMethods sweeteningMethods = productObject.TryGetProperty<SweeteningMethods>("sweeteningMethods");
        bool passoverCertified = productObject.TryGetProperty<bool>("certifiedForPassover");
        bool hypoallergenic = productObject.TryGetProperty<bool>("hypoallergenic");
        bool nonGmo = productObject.TryGetProperty<bool>("nonGmo");
        string nonGmoClaim = productObject.TryGetProperty<string>("nonGmoClaimName");
        string organicClaim = productObject.TryGetProperty<string>("organicClaimName");
        string receiptDescription = productObject.TryGetProperty<string>("receiptDescription");
        string warnings = productObject.TryGetProperty<string>("warnings");
        Restrictions restrictions = productObject.TryGetProperty<Restrictions>("retstrictions");
        IList<Item> items = productObject.TryGetPropertyList<Item>("items");
        ItemInformation itemInformation = productObject.TryGetProperty<ItemInformation>("itemInformation");
        Temperature temperature = productObject.TryGetProperty<Temperature>("temperature");
        string upc = productObject.TryGetProperty<string>("upc");
        Ratings ratings = productObject.TryGetProperty<Ratings>("ratings");
        return new Product(productId,
            productPageUri,
            aliasProductIds,
            brand,
            categories,
            countryOrigin,
            description,
            alcohol,
            alcoholProof,
            ageRestricted,
            snapEligible,
            manufacturerDeclarations,
            sweeteningMethods,
            passoverCertified,
            hypoallergenic,
            nonGmo,
            nonGmoClaim,
            organicClaim,
            receiptDescription,
            warnings,
            restrictions,
            items,
            itemInformation,
            temperature,
            upc,
            ratings
        );
    }
}