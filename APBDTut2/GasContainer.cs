namespace APBD_Tut2;

public class GasContainer(double height, double tareWeight, double depth, double maxPayload, double pressure)
    : Container(height, tareWeight, depth, maxPayload, "G"), IHazardNotifier
{
    public double Pressure { get; private set; } = pressure;

    public override void EmptyCargo()
    {
        double cargoToLeave = maxPayload * 0.05;

        if (CargoWeight - cargoToLeave < 0) return;
        
        CargoWeight = cargoToLeave;
        Mass = TareWeight + CargoWeight;
    }

    public void NotifyHazard(string hazard)
    {
        Console.WriteLine($"WARNING for {SerialNumber}: {hazard}");
    }
}