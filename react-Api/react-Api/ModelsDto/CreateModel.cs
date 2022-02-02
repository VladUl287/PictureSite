using Microsoft.AspNetCore.Http;
using react_Api.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace react_Api.ModelsDto
{
    public class CreateModel
    {
        [Required]
        public IFormFile Picture { get; set; }
        [Required]
        public bool Vertical { get; set; } = false;
        [Required]
        public Tag[] Tags { get; set; }
    }
}
