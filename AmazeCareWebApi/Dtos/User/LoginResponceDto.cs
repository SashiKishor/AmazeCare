namespace AmazeCareWebApi.Dtos.User
{
    public class LoginResponceDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiresAt { get; set; }
        public int? ProfileId { get; set; }
    }
}
