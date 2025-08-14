namespace AuthAPI.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string NmUser { get; set; } = "";
        public string DsPassword { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.User;
    }

    public enum UserRole
    {
        Admin,
        User
    }
}