namespace AmazeCareWebApi.Dtos.PatientDtos
{
    public class PatientCreateDto
    {
        public string FullName { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public int? UserId { get; set; }
    }
}
