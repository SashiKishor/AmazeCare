namespace AmazeCareWebApi.Dtos.AppointmentDtos
{
    public class PatientAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
        public string? Symptomps { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string? Status { get; set; }
        public string? NatureOfVisit { get; set; }
        public int? RecordId { get; set; }
    }
}
