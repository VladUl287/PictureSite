using System.ComponentModel.DataAnnotations;

namespace react_Api.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(150)]
        public string Login { get; set; }
    }
}