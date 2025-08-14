using AuthAPI.Domain;

namespace AuthAPI.Models
{
    public class RegisterUserRequest
    {
        public string NmUser { get; set; } = "";
        public string DsPassword { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.User;
    }
}
