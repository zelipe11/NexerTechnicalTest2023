using System;
using System.Windows.Forms;
using TesteTecnico2023.Model;

namespace TesteTecnico2023.Forms
{
    public partial class TravelAutonomyForm : Form
    {
        private const int MetersInKilometer = 1000;

        public TravelAutonomyForm()
        {
            InitializeComponent();
        }

        private void btnCalculateShipping_Click(object sender, EventArgs e)
        {
            if (TryGetDoubleFromTextBox(txtDistance, out double distance) &&
                TryGetDoubleFromTextBox(txtWeight, out double weight))
            {
                if (Data.FUEL_PRICES.Count == 0)
                {
                    ShowFuelPricesMissingError();
                }
                else
                {
                    Vehicle cheapestVehicle = FindCheapestVehicle(distance, weight);

                    if (cheapestVehicle != null)
                        ShowCheapestVehicleAndCost(cheapestVehicle, CalculateShippingCost(cheapestVehicle, distance));

                    else
                        MessageBox.Show("Nenhum dos veículos cadastrados é ideal para este frete");
                }
            }
            else
            {
                MessageBox.Show("Dados inseridos inválidos");
            }
        }

        private bool TryGetDoubleFromTextBox(TextBox textBox, out double result)
        {
            return double.TryParse(textBox.Text, out result);
        }

        private void ShowFuelPricesMissingError()
           => MessageBox.Show("Cadastre o valor do combustível antes de fazer o cálculo!");

        private Vehicle FindCheapestVehicle(double distance, double weight)
        {
            Vehicle cheapestVehicle = null;
            double minPrice = double.MaxValue;

            foreach (Vehicle vehicle in Data.VEHICLES)
            {
                if (weight <= vehicle.WeightSupported)
                {
                    double shippingCost = CalculateShippingCost(vehicle, distance);

                    if (shippingCost < minPrice)
                    {
                        cheapestVehicle = vehicle;
                        minPrice = shippingCost;
                    }
                }
            }

            return cheapestVehicle;
        }

        private double CalculateShippingCost(Vehicle vehicle, double distance)
        {
            double trips = Math.Ceiling(distance / (vehicle.Autonomy / MetersInKilometer));
            double fuelPrice = (distance / MetersInKilometer) * (vehicle.Autonomy / 100.0) * Data.FUEL_PRICES[0].price;
            double priceInTrip = trips * fuelPrice;
            return priceInTrip;
        }

        private void ShowCheapestVehicleAndCost(Vehicle vehicle, double cost)
        {
            string message = $"Veículo Ideal para frete: {vehicle.Type()}, " +
                             $"{vehicle.Brand}, " +
                             $"{vehicle.ModelName}\n" +
                             $"Seu custo de frete será: R$ {cost:F2}";
            MessageBox.Show(message);
        }
    }
}
