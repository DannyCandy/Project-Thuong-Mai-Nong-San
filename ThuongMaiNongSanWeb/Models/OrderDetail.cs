using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApp.Models
{
    public partial class OrderDetail
    {
        public string OrderDetailId { get; set; } = null!;
        public int Quantity { get; set; }
        public string Idsp { get; set; } = null!;
        [JsonIgnore]
        public string OrderId { get; set; } = null!;
        [Column(TypeName = "decimal(18,0)")]
        public decimal Price { get; set; }
        [JsonIgnore]
        public virtual SanPham IdspNavigation { get; set; } = null!;
        [JsonIgnore]
        //Thêm khóa ngoại nhiều nhiều tới Order
        public virtual Order OrderIdNavigation { get; set; } = null!;


    }
}
