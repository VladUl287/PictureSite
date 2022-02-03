using react_Api.Domain;
using System.Collections.Generic;

namespace react_Api.Database.Models
{
    public class Picture : BaseEntity
    {
        public string Name { get; set; }
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }
        public bool Vertical { get; set; }
        public ICollection<PictureTags> PicturesTags { get; set; }
    }
}