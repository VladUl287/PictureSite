using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace react_Api.Database.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<PictureTags> PicturesTags { get; set; }
    }
}