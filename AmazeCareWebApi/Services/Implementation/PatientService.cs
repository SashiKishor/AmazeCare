using AmazeCareWebApi.Dtos;
using AmazeCareWebApi.Dtos.PatientDtos;
using AmazeCareWebApi.Exceptions;
using AmazeCareWebApi.Exceptions.PatientException;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Interface;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AmazeCareWebApi.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly ILogger<PatientService> _logger;


        public PatientService(IPatientRepository patientRepository,ILogger<PatientService> logger)
        {
            _repository = patientRepository;
            _logger = logger;
        }

        public async Task<(bool Success, string Message, PatientResponceDto? Data)> AddPatient(PatientCreateDto patientCreateDto)
        {
            _logger.LogInformation("Attempting to add a new patient: {PatientName}", patientCreateDto.FullName);
            var data = new Patient { 
                FullName = patientCreateDto.FullName,
                ContactNumber=patientCreateDto.ContactNumber,
                DateOfBirth = patientCreateDto.DateOfBirth,
                Gender = patientCreateDto.Gender,
                UserId = patientCreateDto.UserId
            };
            await _repository.AddPatientAsync(data);
            var responce = TopatientResponceDto(data);

            _logger.LogInformation("Patient added successfully with ID: {PatientId}", data.PatientId);
            return (true, "Patient Added Successfully.", responce);
        }


        public async Task<(bool Success, string Message)> DeletePatientById(int PatientId)
        {
            _logger.LogInformation("Attempting to delete patient with ID: {PatientId}", PatientId);
            try
            {
                if ((await _repository.GetPatientByIdAsync(PatientId))==null)
                {
                    throw new PatientNotFoundException($"Patient record with ID {PatientId} not found.");
                }
                await _repository.DeletePatientByIdAsync(PatientId);

                _logger.LogInformation("Patient record {PatientId} deleted successfully.", PatientId);
                return (true, "Patient Record Deleted Successfully.");
            }
            catch (PatientNotFoundException ex)
            {
                _logger.LogWarning(ex, "Failed to delete patient: {Message}", ex.Message);
                return (false, ex.Message);
            }

        }


        public async Task<List<PatientResponceDto>> GetAllPatients()
        {
            _logger.LogInformation("Fetching all patient records.");
            
            var data=await _repository.GetAllPatientsAsync();
            var result = data.Select(TopatientResponceDto).ToList();

            _logger.LogInformation("Successfully retrieved {Count} patient records.", result.Count);
            return result;
        }

        public async Task<(string Message, PatientResponceDto? Data)> GetPatientById(int PatientId)
        {
            _logger.LogInformation("Fetching patient record for ID: {PatientId}", PatientId);

            try
            {
                var data = await _repository.GetPatientByIdAsync(PatientId);
                if (data == null)
                {
                    throw new PatientNotFoundException($"Patient record {PatientId} not found.");
                }

                _logger.LogInformation("Successfully retrieved patient record for ID: {PatientId}", PatientId);
                return ("Patient Found",TopatientResponceDto(data));
            }
            catch (PatientNotFoundException ex)
            {
                _logger.LogWarning(ex, "Patient not found: {Message}", ex.Message);
                return (ex.Message,null);
            }

        }

        public async Task<(string Message, PatientResponceDto? Data)> UpdatePatient(PatientUpdateDto patients)
        {
            _logger.LogInformation("Attempting to update patient record for ID: {PatientId}", patients.PatientId);
            try {
                var result = await _repository.GetPatientByIdAsync(patients.PatientId);
                if (result == null)
                {
                    throw new PatientNotFoundException($"Patient record {patients.PatientId} not found.");
                }

                result.FullName = patients.FullName;
                result.DateOfBirth = patients.DateOfBirth;
                result.Gender = patients.Gender;
                result.ContactNumber = patients.ContactNumber;
                result.UserId = patients.UserId;

                await _repository.UpdatePatientAsync(result);
                _logger.LogInformation("Patient record for ID {PatientId} updated successfully.", patients.PatientId);
                return ("Updated Patient Sucessfully",TopatientResponceDto(result));
            }
            catch (PatientNotFoundException ex)
            {
                _logger.LogWarning("Patient not found: {Message}", ex.Message);
                return (ex.Message,null);
            }


        }


        private static PatientResponceDto TopatientResponceDto(Patient patient)
        {
            var result = new PatientResponceDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                ContactNumber = patient.ContactNumber,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                UserId = patient.UserId
            };
            return result;
        }


    }
}
