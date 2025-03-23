namespace APBD_Tut2;

public class ContainerShip(double maxSpeed, int maxContainers, double maxWeight)
{
    public List<Container> Containers { get; private set; } = [];
    public double MaxSpeed { get; private set; } = maxSpeed;
    public int MaxContainers { get; private set; } = maxContainers;
    public double MaxWeight { get; private set; } = maxWeight;
    
    public void AddContainer(Container container)
    {
        if (Containers.Count >= MaxContainers) throw new OverfillException("Cannot add more containers");
        if (Containers.Sum(c => c.Mass) + container.Mass > MaxWeight) throw new OverfillException("Cannot add container, ship will be overfilled");
        
        Containers.Add(container);
    }
    
    public void AddMultipleContainers(List<Container> containers)
    {
        if (Containers.Count + containers.Count > MaxContainers) throw new OverfillException("Cannot add more containers");
        if (Containers.Sum(c => c.Mass) + containers.Sum(c => c.Mass) > MaxWeight) throw new OverfillException("Cannot add container, ship will be overfilled");
        
        Containers.AddRange(containers);
    }
    
    public void RemoveContainer(Container container)
    {
        if (!Containers.Contains(container)) throw new ArgumentException("Container not found");
        
        Containers.Remove(container);
    }
    
    public void TransferContainer(Container container, ContainerShip otherShip)
    {
        if (!Containers.Contains(container)) throw new ArgumentException("Container not found");
        otherShip.AddContainer(container);
        Containers.Remove(container);
    }
    
    public void PrintInformation()
    {
        Console.WriteLine($"Ship with max speed: {MaxSpeed}, max containers: {MaxContainers}, max weight: {MaxWeight}");
        Console.WriteLine("Containers:");
        foreach (var container in Containers)
        {
            container.PrintInformation();
        }
    }
}