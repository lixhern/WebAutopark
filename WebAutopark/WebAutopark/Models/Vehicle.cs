namespace WebAutopark.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int Weight {  get; set; }
        public int Year {  get; set; }
        public int Mileage {  get; set; }
        public string Color {  get; set; }
        public double FuelConsumption { get; set; }
    }
}
