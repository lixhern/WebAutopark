using Microsoft.Identity.Client;

namespace WebAutopark.ViewModel
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string Model {  get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
