namespace AmazeCareWebApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Role { get; set; } = "Patient";

        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }
}
