using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Exceptions.DoctorExceptions;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Interface;
using log4net.Filter;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AmazeCareWebApi.Services.Implementation
{
    public class DoctorService:IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(IDoctorRepository repository, ILogger<DoctorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<(bool Success, string Message, DoctorResponceDto? Data)> AddDoctorAsync(DoctorCreateDto doctorCreateDto)
        {
            _logger.LogInformation("Attempting to add a new doctor: {DoctorName}", doctorCreateDto.DoctorName);
            
            var data = new Doctor
            {
                DoctorName = doctorCreateDto.DoctorName,
                Designation = doctorCreateDto.Designation,
                Experience = doctorCreateDto.Experience,
                Qualification = doctorCreateDto.Qualification,
                Speciality = doctorCreateDto.Speciality,
                UserId = doctorCreateDto.UserId
            };

            _logger.LogInformation("Doctor {DoctorName} added successfully with ID: {DoctorId}", data.DoctorName, data.DoctorId);
            await _repository.AddDoctorAsync(data);
            return (true, "Doctor record added.", ToDataResponceDto(data));

        }

        public async Task<(bool Success, string Message)> DeleteDoctorByIdAsync(int DoctorId)
        {
            _logger.LogInformation("Attempting to delete doctor with ID: {DoctorId}", DoctorId);
            try
            {
                var data = await _repository.GetDoctorByIdAsync(DoctorId);
                if (data == null)
                {
                    throw new DoctorNotFoundException($"Doctor record {DoctorId} not found.");
                }
                await _repository.DeleteDoctorByIdAsync(DoctorId);
                _logger.LogInformation("Doctor with ID {DoctorId} deleted successfully.", DoctorId);
                return (true, "Doctor data deleted.");
            }
            catch(DoctorNotFoundException ex)
            {
                _logger.LogWarning("Deletion failed. Doctor with ID {DoctorId} not found.", DoctorId);
                return (false, ex.Message);
            }
        }

        public async Task<List<DoctorResponceDto>> GetAllDoctorsAsync()
        {
            _logger.LogInformation("Fetching all doctor records.");
            var result = await _repository.GetAllDoctorsAsync();
            _logger.LogInformation("Successfully retrieved {Count} doctor records.", result.Count);
            return result.Select(ToDataResponceDto).ToList();
        }

        public async Task<(string Message, DoctorResponceDto? Data)> GetDoctorByIdAsync(int DoctorId)
        {
            _logger.LogInformation("Fetching doctor record for ID: {DoctorId}", DoctorId);
            try
            {
                var result = await _repository.GetDoctorByIdAsync(DoctorId);
                if (result == null)
                {
                    throw new DoctorNotFoundException($"Doctor record {DoctorId} not found.");
                }

                _logger.LogInformation("Successfully retrieved doctor record for ID: {DoctorId}", DoctorId);
                return ("Doctor Found",ToDataResponceDto(result));
            }
            catch (DoctorNotFoundException ex)
            {
                _logger.LogWarning("Doctor record with ID {DoctorId} not found.", DoctorId);
                return (ex.Message,null);
            }

        }

        public async Task<(bool Success, string Message, DoctorResponceDto? data)> UpdateDoctorDetailsAsync(DoctorUpdateDto doctorUpdateDto)
        {
            _logger.LogInformation("Attempting to update doctor record for ID: {DoctorId}", doctorUpdateDto.DoctorId);
            try
            {
                var result = await _repository.GetDoctorByIdAsync(doctorUpdateDto.DoctorId);
                if (result == null)
                {
                    throw new DoctorNotFoundException($"Doctor record {doctorUpdateDto.DoctorId} not found.");
                }
                result.DoctorName = doctorUpdateDto.DoctorName;
                result.Designation = doctorUpdateDto.Designation;
                result.Qualification = doctorUpdateDto.Qualification;
                result.Experience = doctorUpdateDto.Experience;
                result.Speciality = doctorUpdateDto.Speciality;
                result.UserId = doctorUpdateDto.UserId;

                await _repository.UpdateDoctorRecordAsync(result);

                _logger.LogInformation("Doctor record for ID {DoctorId} updated successfully.", doctorUpdateDto.DoctorId);

                return (true, "Doctor Updated Successfully", ToDataResponceDto(result));
            }
            catch (DoctorNotFoundException ex)
            {
                _logger.LogWarning("Update failed. Doctor with ID {DoctorId} not found.", doctorUpdateDto.DoctorId);
                return (false, ex.Message, null);
            }

        }

        public async Task<(bool Sucess,string Message,List<DoctorResponceDto>? data)> GetAvailableDoctorsAsync(DoctorAvailabiltyRequest request)
        {
            _logger.LogInformation("Fetching available doctors based on requested filters.");

            try
            {
                var query = _repository.GetAllDoctorsQueryable();
                query = applyFilter(query, request);
                query = query.OrderBy(a => a.DoctorId);
                var data = await query.ToListAsync();

                if (data == null)
                {
                    throw new NoAvailableDoctorsException("No doctors available matching the requested criteria.");   
                }

                _logger.LogInformation("Found {Count} available doctors matching the criteria.", data.Count);
                return (true, "Available Doctor:", data.Select(ToDataResponceDto).ToList());
            }
            catch(NoAvailableDoctorsException ex)
            {
                _logger.LogInformation("No doctors available matching the requested criteria.");
                return (false, ex.Message, null);
            }
            
        }
        

        private IQueryable<Doctor> applyFilter(IQueryable<Doctor> query, DoctorAvailabiltyRequest request)
        {
            if (request.PreferedSlot.HasValue)
            {
                DateTime requestedStart = request.PreferedSlot.Value;
                DateTime requestedEnd = requestedStart.AddMinutes(30);

                DateOnly requestedDate = DateOnly.FromDateTime(requestedStart);
                TimeOnly requestedTimeStart = TimeOnly.FromDateTime(requestedStart);
                TimeOnly requestedTimeEnd = TimeOnly.FromDateTime(requestedEnd);

                query = query.Where(d => !d.Appointments!.Any(a =>a.AppointmentDate == requestedDate &&
                    a.PreferedTime < requestedTimeEnd &&
                    a.PreferedTime.AddMinutes(30) > requestedTimeStart
                ));
            }
            if (!string.IsNullOrWhiteSpace(request.DoctorName))
            {
                query = query.Where(d => d.DoctorName.Contains(request.DoctorName));
            }
            if (!string.IsNullOrWhiteSpace(request.Speciality))
            {
                query = query.Where(a => a.Speciality == request.Speciality);
            }
            if (!string.IsNullOrWhiteSpace(request.Qualification))
            {
                query = query.Where(a => a.Qualification == request.Qualification);
            }
            if (request.Experience.HasValue)
            {
                query = query.Where(a => a.Experience >= request.Experience);
            }

            return query;
        }

        private static DoctorResponceDto ToDataResponceDto(Doctor doctors)
        {
            return new DoctorResponceDto
            {
                DoctorId = doctors.DoctorId,
                Designation = doctors.Designation,
                DoctorName = doctors.DoctorName,
                Experience = doctors.Experience,
                Qualification = doctors.Qualification,
                Speciality = doctors.Speciality,
                UserId = doctors.UserId
            };
        }
    }
}
