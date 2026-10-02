namespace Anchises.KrogerAPI;

public static class KrogerUri
{
    public const string DevPortal = "https://developer.kroger.com/";

    public static bool IsProduction { get; private set; } = true;
    
    public static string Token => IsProduction ? ProductionToken : TestToken; 
    
    public const string ProductionToken = "https://api.kroger.com/v1/connect/oauth2/token";
    
    public const string TestToken = "https://api-ce.kroger.com/v1/connect/oauth2/token";
    
    public static string Locations => IsProduction ? ProductionLocations : TestLocations; 
    
    public const string ProductionLocations = "https://api.kroger.com/v1/locations";

    public const string TestLocations = "https://api-ce.kroger.com/v1/locations";

    public static string Products => IsProduction ?  ProductionProducts : TestProducts;
    
    public const string ProductionProducts = "https://api.kroger.com/v1/products";
    
    public const string TestProducts = "https://api-ce.kroger.com/v1/products";
}