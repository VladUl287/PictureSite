using react_Api.Database.Models;
using System.Collections.Generic;

namespace react_Api.ViewModels
{
    public class PictureModel
    {
        public int Id { get; set; }
        public string View { get; set; }
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }
        public bool Vertical { get; set; } = false;
        public ICollection<Tag> Tags { get; set; }
    }
}