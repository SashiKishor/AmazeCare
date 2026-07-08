namespace AmazeCareWebApi.Dtos.AppointmentDtos
{
    public class AppointmentRescheduleDto
    {
        public int AppointmentId { get; set; }
        public DateTime RescheduledSlot { get; set; }
    }
}
