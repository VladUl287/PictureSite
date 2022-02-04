using react_Api.Domain;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace react_Api.Database.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<PictureTags> PicturesTags { get; set; }
    }
}