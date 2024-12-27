using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public partial class ThongTinLienHe
    {
        public string UserId { get; set; } = null!;
        public string Idttlh { get; set; } = null!;
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^((03[2-9]|05[6|8|9]|07[0|6|7|8|9]|08[1-9]|09[1-4|6-9])[0-9]{7})$", ErrorMessage = "Viet Nam phone number must be 10 or 11 digits.")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc.")]
        public string? DcgiaoHang { get; set; }
        public bool DefaultPos { get; set; } = false;

        //Thêm khóa ngoại ánh xạ tới User
        /*public ApplicationUser User { get; set; } = null!;*/
        public ApplicationUser? User { get; set; }
    }
}
