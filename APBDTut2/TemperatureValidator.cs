namespace APBD_Tut2;

public static class TemperatureValidator
{
    public static bool IsValidTemperature(ProductType product, double temperature)
    {
        var temperatureForProduct = product switch
        {
            ProductType.Bananas => 13.3,
            ProductType.Chocolate => 18,
            ProductType.Meat => -15,
            ProductType.Fish => 2,
            ProductType.IceCream => -18,
            ProductType.FrozenPizza => -30,
            ProductType.Cheese => 7.2,
            ProductType.Sausages => 5,
            ProductType.Butter => 20.5,
            ProductType.Eggs => 19,
            _ => throw new ArgumentException("Invalid product type")
        };
        
        return temperature >= temperatureForProduct;
    }
    
}