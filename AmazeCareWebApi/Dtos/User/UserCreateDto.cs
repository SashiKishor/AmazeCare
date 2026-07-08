namespace AmazeCareWebApi.Dtos.User
{
    public class UserCreateDto
    {
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
    }
}
