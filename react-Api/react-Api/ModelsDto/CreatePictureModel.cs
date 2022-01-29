using Microsoft.AspNetCore.Http;
using react_Api.Database.Models;
using System.ComponentModel.DataAnnotations;

namespace react_Api.ModelsDto
{
    public class CreatePictureModel
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public Tag[] Tags { get; set; }
    }
}