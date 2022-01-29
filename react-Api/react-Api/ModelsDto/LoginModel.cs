using System.ComponentModel.DataAnnotations;

namespace react_Api.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}