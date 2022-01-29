using System.Text.Json.Serialization;

namespace react_Api.Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}