namespace TesteTecnico2023.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    abstract class Vehicle
    {
        public string ModelName { get; set; }
        public string Brand { get; set; }
        public int Wheels { get; set; }
        public double WeightSupported { get; set; }
        public double Autonomy { get; set; }
        public string FuelType { get; set; }

        public abstract string Type();

        public double GetShippingCalculation(double fuelPrice, double weight, double distance)
        {
            double fuelRequired = 0d;

            if (this.WeightSupported >= weight)    
                fuelRequired = distance / this.Autonomy;

            else
            {
                double weightFactor = weight / this.WeightSupported;
                int shipments = (int)Math.Ceiling(weightFactor);
                double distancePerShipment = distance / this.Autonomy;
                fuelRequired = shipments * distancePerShipment;
            }

            double shippingCalculation = fuelRequired * fuelPrice;

            return shippingCalculation;
        }


        public virtual void showInfo()
        {
            Console.WriteLine($"Marca: {Brand}, Modelo: {ModelName}, Rodas: {Wheels}");
        }

        public Vehicle()
        {
        }
    }
}
