namespace APBD_Tut2;

public class LiquidContainer(double height, double tareWeight, double depth, double maxPayload, bool isHazardous)
    : Container(height, tareWeight, depth, maxPayload, "L"), IHazardNotifier
{   
    public bool IsHazardous { get; private set; } = isHazardous;

    public override void LoadCargo(double loadWeight)
    {
        var maxAllowedLoad = IsHazardous ? MaxPayload * 0.5 : MaxPayload * 0.9;
        
        if (CargoWeight + loadWeight > maxAllowedLoad)
        {
            NotifyHazard($"Overfilling the container. Current load is: {CargoWeight}kg. Attempted to load {loadWeight}kg, " +
                         $"but the container should only handle up to {maxAllowedLoad}kg.");
        }
        base.LoadCargo(loadWeight);
    }

    public void NotifyHazard(string hazard)
    {
        Console.WriteLine($"WARNING for {SerialNumber}: {hazard}");
    }
}