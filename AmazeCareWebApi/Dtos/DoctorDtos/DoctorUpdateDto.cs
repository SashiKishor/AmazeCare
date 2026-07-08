namespace AmazeCareWebApi.Dtos.DoctorDtos
{
    public class DoctorUpdateDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;
        public string Speciality { get; set; } = null!;
        public float Experience { get; set; }
        public string Qualification { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public int? UserId { get; set; }
    }
}
