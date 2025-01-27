using System.ComponentModel.DataAnnotations;

namespace WebAutopark.Models
{
    public class VehicleType
    {
        public int VehicleTypeId { get; set; }
        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number.")]
        public float TaxCoefficient { get; set; }
    }
}
