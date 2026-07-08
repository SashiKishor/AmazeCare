using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task<bool> AddAppointmentRecordsAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<bool> DeleteAppointmentAsync(int appointmentId);
        Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);
        Task UpdateAppointment(Appointment appointment);
        IQueryable<Appointment> GetAppointmentRecordsQueryable();
    }
}
