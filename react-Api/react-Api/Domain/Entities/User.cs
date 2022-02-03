using react_Api.Domain;

namespace react_Api.Database.Models
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
    }
}