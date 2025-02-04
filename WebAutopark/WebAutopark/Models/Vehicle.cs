using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAutopark.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        [Required]
        [Remote(action: "CheckVehicleTypeId", controller:"VehicleType", ErrorMessage = "There are no such vehicle type id")]
        public int VehicleTypeId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Model { get; set; }
        [Required]
        [Remote(action: "CheckForRegNumber", controller:"Vehicle", AdditionalFields = nameof(Model), ErrorMessage ="There is already exist vehicle with this reg number")]
        public string RegistrationNumber { get; set; }
        [Required]
        [Range(500, 30000, ErrorMessage ="The weight must be between 500 and 30000")]
        public int Weight {  get; set; }
        [Required]
        [Range(1885, 2025, ErrorMessage = "The year must be berween 1885 and 2025")]
        public int Year {  get; set; }
        [Required]
        public int Mileage {  get; set; }
        [Required]
        [MinLength(3)]
        public string Color {  get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="The fuel consumption cannot be less than 1")]
        public double FuelConsumption { get; set; }
    }
}
