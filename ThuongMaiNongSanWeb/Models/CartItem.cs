using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class CartItem
    {
        public string ProductImg { get; set; }
        public string ProductId { get; set; }
        public string? Name { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
