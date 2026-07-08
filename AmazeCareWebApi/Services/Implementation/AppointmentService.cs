using AmazeCareWebApi.Dtos.AppointmentDtos;
using AmazeCareWebApi.Exceptions.AppointmentException;
using AmazeCareWebApi.Exceptions.DoctorExceptions;
using AmazeCareWebApi.Exceptions.PatientException;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;


namespace AmazeCareWebApi.Services.Implementation
{
    public class AppointmentService : IAppotimentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository appointmentRepository, IPatientService patientService, IDoctorService doctorService, ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _doctorService = doctorService;
            _patientService = patientService;
            _logger = logger;
        }

        public async Task<(bool Success,string Message,AppointmentResponceDto? data)> AddAppointment(AppointmentCreateDto appointmentCreateDto)
        {
            _logger.LogInformation("Attempting to add appointment for Patient ID {PatientId} with Doctor ID {DoctorId}", appointmentCreateDto.PatientId, appointmentCreateDto.DoctorId);
            try
            {
                var patient = await _patientService.GetPatientById(appointmentCreateDto.PatientId);
                var doctor = await _doctorService.GetDoctorByIdAsync(appointmentCreateDto.DoctorId);

                if (patient.Data == null)
                {
                    throw new PatientNotFoundException($"Patient with ID {appointmentCreateDto.PatientId} not found.");
                }
                if (doctor.Data == null)
                {
                    throw new DoctorNotFoundException($"Doctor with ID {appointmentCreateDto.DoctorId} not found.");
                }
                var data = new Appointment
                {
                    AppointmentDate = appointmentCreateDto.AppointmentDate,
                    DoctorId = appointmentCreateDto.DoctorId,
                    NatureOfVisit = appointmentCreateDto.NatureOfVisit,
                    PatientId = appointmentCreateDto.PatientId,
                    PreferedTime = appointmentCreateDto.PreferedTime,
                    Status = appointmentCreateDto.Status,
                    SymptomsDescription = appointmentCreateDto.SymptomsDescription
                };

                var result = await _appointmentRepository.AddAppointmentRecordsAsync(data);

                _logger.LogInformation("Appointment successfully added with ID: {AppointmentId}", data.AppointmentId);
                return (true, "Appointment Added Successfully.", ToAppointmentResponceDto(data));
            }
            catch (PatientNotFoundException ex) {
                _logger.LogWarning(ex, "Failed to add appointment: {Message}", ex.Message);
                return (false, ex.Message, null);
            }
            catch(DoctorNotFoundException ex)
            {
                _logger.LogWarning(ex, "Failed to add appointment: {Message}", ex.Message);
                return (false, ex.Message, null);
            }
        }

        public async Task<List<AppointmentResponceDto>> GetAllAppointments()
        {
            _logger.LogInformation("Fetching all appointments.");

            var data=await _appointmentRepository.GetAllAppointmentsAsync();
            
            _logger.LogInformation("Appointments retrived sucessfully");
            return data.Select(ToAppointmentResponceDto).ToList();

        }

        public async Task<(bool Success, string Message, AppointmentResponceDto? data)> GetAppointmentById(int id)
        {
            _logger.LogInformation("Fetching appointment by ID: {Id}", id);
            try
            {
                var result = await _appointmentRepository.GetAppointmentByIdAsync(id);
                if (result == null)
                {
                    throw new AppointmentNotFoundException($"Appointment with ID {id} not found.");
                }
                return (true,"Appointments Retrived",ToAppointmentResponceDto(result));
            }
            catch (AppointmentNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false,ex.Message,null);
            }
        }

        public async Task<(bool Success, string Message)> RemoveAppointment(int id)
        {
            _logger.LogInformation("Attempting to remove appointment ID: {Id}", id);
            try
            {
                var result = await _appointmentRepository.DeleteAppointmentAsync(id);
                if (!result)
                {
                    throw new AppointmentNotFoundException($"Appointment with ID {id} not found.");
                }
                return (true, "Appointment Removed");
            }
            catch(AppointmentNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message);
            }
        }


        public async Task<(bool sucess, string message, List<DoctorAppointmentDto>? data)> GetAppointmentByDoctor(int doctorId)
        {
            _logger.LogInformation("Fetching appointments for Doctor ID: {DoctorId}", doctorId);
            try
            {
                var query = _appointmentRepository.GetAppointmentRecordsQueryable();
                var data = await query.Where(a => a.DoctorId == doctorId).ToListAsync();
                if (data == null || !data.Any())
                {
                    throw new NoAppointmentYetException($"No appointments found for Doctor ID {doctorId}.");
                }
                var result=data.Select(a => new DoctorAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.PreferedTime,
                    ContactNo = a.Patient?.ContactNumber,
                    PatientId = a.PatientId,
                    PatientName = a.Patient?.FullName,
                    Status = a.Status,
                    Symptomps = a.SymptomsDescription,
                    RecordId = a.MedicalRecord?.RecordId
                }).ToList();
                return (true, "Doctors All appointment retrived", result);
            }
            catch (NoAppointmentYetException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message, new List<DoctorAppointmentDto>());
            }
             
        }

        public async Task<(bool sucess, string message, List<DoctorAppointmentDto>? data)> GetUpcomingAppointmentForDoctor(int doctorId)
        {
            _logger.LogInformation("Fetching appointments for Doctor ID: {DoctorId}", doctorId);
            try
            {
                var query = _appointmentRepository.GetAppointmentRecordsQueryable();
                var data = await query.Where(a => a.DoctorId == doctorId && a.Status == "Upcoming").ToListAsync();
                if (data == null || !data.Any())
                {
                    throw new NoAppointmentYetException($"No Upcoming appointments found for Doctor ID {doctorId}.");
                }
                var result = data.Select(a => new DoctorAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.PreferedTime,
                    ContactNo = a.Patient?.ContactNumber,
                    PatientId = a.PatientId,
                    PatientName = a.Patient?.FullName,
                    Status = a.Status,
                    Symptomps = a.SymptomsDescription,
                    RecordId = a.MedicalRecord?.RecordId
                }).ToList();
                return (true, "Doctors All appointment retrived", result);
            }
            catch (NoAppointmentYetException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message, new List<DoctorAppointmentDto>());
            }
        }


        public async Task<(bool Success, string Message, List<PatientAppointmentDto>? data)> GetAppointmentByPatient(int patientId)
        {
            _logger.LogInformation("Fetching appointments for Patient ID: {PatientId}", patientId);
            try
            {
                var query = _appointmentRepository.GetAppointmentRecordsQueryable();
                var data = await query.Where(a => a.PatientId == patientId).ToListAsync();
                if (data == null || !data.Any())
                {
                    throw new NoAppointmentYetException($"No appointments found for Patient ID {patientId}.");
                }

                var result = data.Select(a => new PatientAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.PreferedTime,
                    DoctorName = a.Doctor?.DoctorName,
                    PatientName = a.Patient?.FullName,
                    Status = a.Status,
                    Symptomps = a.SymptomsDescription,
                    RecordId = a.MedicalRecord?.RecordId,
                    NatureOfVisit = a.NatureOfVisit
                }).ToList();
                return (true, "Appointments retrieved.", result);
            }
            catch (NoAppointmentYetException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message, new List<PatientAppointmentDto>());
            }

            
        }

        public async Task<(bool Success, string Message, List<PatientAppointmentDto>? data)> GetUpcomingAppointmentForPatient(int patientId)
        {
            _logger.LogInformation("Fetching appointments for Patient ID: {PatientId}", patientId);
            try
            {
                var query = _appointmentRepository.GetAppointmentRecordsQueryable();
                var data = await query.Where(a => a.PatientId == patientId && a.Status == "Upcoming").ToListAsync();
                if (data == null || !data.Any())
                {
                    throw new NoAppointmentYetException($"No appointments found for Patient ID {patientId}.");
                }

                var result = data.Select(a => new PatientAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.PreferedTime,
                    DoctorName = a.Doctor?.DoctorName,
                    PatientName = a.Patient?.FullName,
                    Status = a.Status,
                    Symptomps = a.SymptomsDescription,
                    RecordId = a.MedicalRecord?.RecordId
                }).ToList();
                return (true, "Appointments retrieved.", result);
            }
            catch (NoAppointmentYetException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message, new List<PatientAppointmentDto>());
            }
        }



        public async Task<(bool Success, string Message, List<AppointmentResponceDto>? data)> GetAllUpcomingAppointments()
        {
            _logger.LogInformation("Fetching all upcoming appointments.");
            try
            {
                var query = _appointmentRepository.GetAppointmentRecordsQueryable();
                var data = await query.Where(a => a.Status == "Upcoming" && a.AppointmentDate >= DateOnly.FromDateTime(DateTime.Now)).ToListAsync();
                if (data == null || !data.Any())
                {
                    throw new NoAppointmentYetException("No Upcoming appointment found.");
                }
                return (true, "Upcoming Appointments found", data.Select(ToAppointmentResponceDto).ToList());
            }
            catch (NoAppointmentYetException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message, new List<AppointmentResponceDto>());
            }
            
        }

        public async Task<(bool Success, string Message, List<AppointmentResponceDto>? data)> GetAllRequestedAppointments()
        {
            _logger.LogInformation("Fetching all Requested appointments.");
            try
            {
                var query = _appointmentRepository.GetAppointmentRecordsQueryable();
                var data = await query.Where(a => a.Status == "Requested").ToListAsync();
                if (data == null || !data.Any())
                {
                    throw new NoAppointmentYetException("No Requested appointment found.");
                }
                return (true, "Requested Appointments found", data.Select(ToAppointmentResponceDto).ToList());
            }
            catch (NoAppointmentYetException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message, new List<AppointmentResponceDto>());
            }
        }


        public async Task<(bool Success, string Message, AppointmentResponceDto? data)> UpdateAppointmentStatus(AppointmentUpdateStatusDto statusDto)
        {
            _logger.LogInformation("Updating status for appointment ID: {AppointmentId}", statusDto.AppointmentId);
            try
            {
                var data = await _appointmentRepository.GetAppointmentByIdAsync(statusDto.AppointmentId);
                if (data == null)
                {
                    throw new AppointmentNotFoundException($"Appointment with ID {statusDto.AppointmentId} not found.");
                }
                data.Status = statusDto.Status;
                await _appointmentRepository.UpdateAppointment(data);
                return (true, "Status updated successfully.", ToAppointmentResponceDto(data));
            }
            catch(AppointmentNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return(false, ex.Message, null);
            }
            
        }

        public async Task<(bool Success, string Message, AppointmentResponceDto? data)> RescheduleAppointment(AppointmentRescheduleDto rescheduleDto)
        {
            _logger.LogInformation("Rescheduling appointment ID: {AppointmentId}", rescheduleDto.AppointmentId);
            try
            {
                var data = await _appointmentRepository.GetAppointmentByIdAsync(rescheduleDto.AppointmentId);
                var date = DateOnly.FromDateTime(rescheduleDto.RescheduledSlot);
                var time = TimeOnly.FromDateTime(rescheduleDto.RescheduledSlot);
                if (data == null)
                {
                    throw new AppointmentNotFoundException($"Appointment with ID {rescheduleDto.AppointmentId} not found.");
                }
                data.Status = "Rescheduled";
                data.AppointmentDate = date;
                data.PreferedTime = time;
                await _appointmentRepository.UpdateAppointment(data);
                return (true, "Appointment rescheduled successfully.", ToAppointmentResponceDto(data));
            }
            catch (AppointmentNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return (false, ex.Message, null);
            }

        }

        private static AppointmentResponceDto ToAppointmentResponceDto(Appointment appointment)
        {
            return new AppointmentResponceDto
            {
                AppointmentId = appointment.AppointmentId,
                AppointmentDate = appointment.AppointmentDate,
                SymptomsDescription = appointment.SymptomsDescription,
                Status = appointment.Status,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient?.FullName,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor?.DoctorName,
                NatureOfVisit = appointment.NatureOfVisit,
                PreferedTime = appointment.PreferedTime,
                RecordId = appointment.MedicalRecord?.RecordId
            };
        }

    }
}
