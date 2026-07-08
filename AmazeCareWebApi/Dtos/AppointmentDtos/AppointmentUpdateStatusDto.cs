namespace AmazeCareWebApi.Dtos.AppointmentDtos
{
    public class AppointmentUpdateStatusDto
    {
        public int AppointmentId { get; set; }
        public string Status { get; set; } = null!;
    }
}
