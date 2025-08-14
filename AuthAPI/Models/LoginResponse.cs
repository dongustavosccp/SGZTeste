namespace AuthAPI.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public string Token { get; set; } = "";
        public Guid UserId { get; set; }
        public string NmUser { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
