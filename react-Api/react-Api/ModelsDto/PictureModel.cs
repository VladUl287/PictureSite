using react_Api.Database.Models;
using System.Collections.Generic;

namespace react_Api.ModelsDto
{
    public class PictureModel
    {
        public int Id { get; set; }
        public string View { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}