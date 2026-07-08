using AmazeCareWebApi.Dtos.AppointmentDtos;
using AmazeCareWebApi.Models;

namespace AmazeCareWebApi.Services.Interface
{
    public interface IAppotimentService
    {
        Task<(bool Success, string Message, AppointmentResponceDto? data)> GetAppointmentById(int id);
        Task<(bool Success, string Message,AppointmentResponceDto? data)> AddAppointment(AppointmentCreateDto appointmentCreateDto);
        Task<(bool Success, string Message)> RemoveAppointment(int id);
        Task<List<AppointmentResponceDto>> GetAllAppointments();
        Task<(bool sucess, string message, List<DoctorAppointmentDto>? data)> GetAppointmentByDoctor(int doctorId);
        Task<(bool sucess, string message, List<DoctorAppointmentDto>? data)> GetUpcomingAppointmentForDoctor(int doctorId);
        Task<(bool Success, string Message, List<PatientAppointmentDto>? data)> GetAppointmentByPatient(int patientId);
        Task<(bool Success, string Message, List<PatientAppointmentDto>? data)> GetUpcomingAppointmentForPatient(int patientId);
        Task<(bool Success, string Message, AppointmentResponceDto? data)> UpdateAppointmentStatus(AppointmentUpdateStatusDto statusDto);
        Task<(bool Success, string Message, List<AppointmentResponceDto>? data)> GetAllUpcomingAppointments();
        Task<(bool Success, string Message, List<AppointmentResponceDto>? data)> GetAllRequestedAppointments();
        Task<(bool Success, string Message, AppointmentResponceDto? data)> RescheduleAppointment(AppointmentRescheduleDto rescheduleDto);

    }
}
