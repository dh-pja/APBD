using System;

namespace APBD_Tut2
{
    class Program
    {
        public static void Main(string[] args)
        {
            var ship = new ContainerShip(80.0, 5, 400.0);
            Container container1 = new LiquidContainer(120.0, 60.0, 110.0, 110.0, false);
            container1.LoadCargo(40.0);
            ship.AddContainer(container1);
            Container container2 = new GasContainer(100.0, 50.0, 90.0, 90.0, 100.0);
            container2.LoadCargo(40.0);
            ship.AddContainer(container2);
            ship.PrintInformation();
        }
    }
}

