using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.MobileApp.DTO
{
    public class OrderRequest
    {
        [Required]
        public ShoppingCart Cart { get; set; } = null!;
        public string? Message { get; set; }
    }
}
