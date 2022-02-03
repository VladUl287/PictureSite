namespace react_Api.Database.Models
{
    public class PictureTags
    {
        public int PictureId { get; set; }
        public Picture Picture { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}