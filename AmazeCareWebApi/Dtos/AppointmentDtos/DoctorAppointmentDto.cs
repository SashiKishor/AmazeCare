namespace AmazeCareWebApi.Dtos.AppointmentDtos
{
    public class DoctorAppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? ContactNo { get; set; }
        public string? Symptomps { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string? Status { get; set; } 
        public int? RecordId { get; set; }

    }
}
