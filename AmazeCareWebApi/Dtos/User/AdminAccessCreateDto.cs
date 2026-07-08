using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Dtos.User
{
    public class AdminAccessCreateDto
    {
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Role { get; set; } = "Doctor";
        public string? Speciality { get; set; }
        public float? Experience { get; set; }
        public string? Qualification { get; set; }
        public string? Designation { get; set; }
    }
}
