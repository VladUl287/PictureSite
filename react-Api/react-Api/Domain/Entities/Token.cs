using react_Api.Domain;

namespace react_Api.Database.Models
{
    public class Token : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string RefreshToken { get; set; }
    }
}