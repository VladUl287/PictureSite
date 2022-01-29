using System.Collections.Generic;

namespace react_Api.Database.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public ICollection<PictureTags> PicturesTags { get; set; }
    }
}