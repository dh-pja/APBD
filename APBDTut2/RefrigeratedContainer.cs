namespace APBD_Tut2;

public class RefrigeratedContainer(double height, double tareWeight, double depth, double maxPayload, ProductType productType, double temperature) 
    : Container(height, tareWeight, depth, maxPayload, "R")
{
    public ProductType Product { get; private set; } = productType;
    public double Temperature { get; private set; } = TemperatureValidator.IsValidTemperature(productType, temperature) ? temperature : throw new ArgumentException("Invalid temperature for the provided product type");
    
}