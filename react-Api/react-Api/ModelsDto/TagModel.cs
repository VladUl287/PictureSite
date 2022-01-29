using System.ComponentModel.DataAnnotations;

namespace react_Api.ModelsDto
{
    public class TagModel
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
    }
}