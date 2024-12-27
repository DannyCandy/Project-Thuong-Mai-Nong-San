using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ForgotPassword
    {
        [Required, EmailAddress, Display(Name = "Cần nhập email đã đăng kí")]
        public string? Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
