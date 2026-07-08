namespace AmazeCareWebApi.Dtos.DoctorDtos
{
    public class DoctorAvailabiltyRequest
    {
        public DateTime? PreferedSlot { get; set; }
        public string? DoctorName { get; set; }
        public string? Speciality { get; set; }
        public float? Experience { get; set; }
        public string? Qualification { get; set; } 
    }
}
