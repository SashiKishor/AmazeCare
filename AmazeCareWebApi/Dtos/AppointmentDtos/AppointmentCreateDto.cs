namespace AmazeCareWebApi.Dtos.AppointmentDtos
{
    public class AppointmentCreateDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly PreferedTime { get; set; }
        public string SymptomsDescription { get; set; } = string.Empty;
        public string NatureOfVisit { get; set; } = null!;
        public string Status { get; set; } = "Requested";
    }
}
