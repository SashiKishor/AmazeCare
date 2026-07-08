using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.PatientDtos;

namespace AmazeCareWebApi.Dtos.AppointmentDtos
{
    public class AppointmentResponceDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly PreferedTime { get; set; }
        public string? SymptomsDescription { get; set; } 
        public string? NatureOfVisit { get; set; }
        public string? Status { get; set; } 
        public int? RecordId { get; set; }
    }
}
