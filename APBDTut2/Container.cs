namespace APBD_Tut2;

public abstract class Container
{
    public double Mass { get; protected set;  }
    protected double Height { get; }
    protected double TareWeight { get; }
    protected double CargoWeight { get; set; }
    protected double Depth { get; }
    protected string SerialNumber { get; }
    protected double MaxPayload { get; }

    protected Container(double height, double tareWeight, double depth, double maxPayload, string containerType)
    {
        Mass = tareWeight;
        Height = height;
        TareWeight = tareWeight;
        CargoWeight = 0;
        Depth = depth;
        MaxPayload = maxPayload;
        SerialNumber = GenerateSerialNumber(containerType);
    }
    
    private static string GenerateSerialNumber(string containerType)
    {
        return $"KON-{containerType}-{new Random().Next(10000, 99999)}";
    }
    
    public virtual void EmptyCargo()
    {
        CargoWeight = 0.0;
        Mass = TareWeight;
    }

    public virtual void LoadCargo(double weight)
    {
        if (CargoWeight + weight > MaxPayload)
        {
            throw new OverfillException($"Cannot load more cargo than the container can handle. " +
                                        $"Current load is: {CargoWeight}" +
                                        $"Attempted to load {weight}kg, but the container can only handle {MaxPayload}kg.");
        }
        
        CargoWeight += weight;
        Mass = TareWeight + CargoWeight;
    }
    
    public void PrintInformation()
    {
        Console.WriteLine($"Container {SerialNumber}:" +
                          $"\n Height: {Height}" +
                          $"\n Depth: {Depth}" +
                          $"\n Tare weight: {TareWeight}" +
                          $"\n Max payload: {MaxPayload}" +
                          $"\n Current cargo weight: {CargoWeight}" +
                          $"\n Current mass: {Mass}");
    }
}