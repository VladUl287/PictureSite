using react_Api.Domain;
using System.Collections.Generic;

namespace react_Api.Database.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<PictureTags> PicturesTags { get; set; }
    }
}