using AmazeCareWebApi.Dtos.PrescriptionDtos;
using AmazeCareWebApi.Exceptions.MedicalRecordException;
using AmazeCareWebApi.Exceptions.PerscriptionException;
using AmazeCareWebApi.Migrations;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Interface;

namespace AmazeCareWebApi.Services.Implementation
{
    public class PescriptionService : IPerscriptionService
    {
        private readonly IPrescriptionRepository _repository;
        private readonly ILogger<PescriptionService> _logger;
        private readonly IMedicalRecordService _recordService;

        public PescriptionService(IPrescriptionRepository repository, ILogger<PescriptionService> logger, IMedicalRecordService medicalRecordService)
        {
            _repository=repository;
            _logger=logger;
            _recordService=medicalRecordService;
        }

        public async Task<(bool sucess, string message)> AddPrescription(PrescriptionCreateDto prescriptions)
        {
            _logger.LogInformation("Attempting to add a new prescription for Record ID: {RecordId}",prescriptions.RecordId);
            try
            {
                var medicalrecord=await _recordService.GetMedicalRecordByIdAsync(prescriptions.RecordId);
                if (medicalrecord.Data == null)
                {
                    throw new MedicalRecordNotFoundException($"No medical record found for {prescriptions.RecordId}");
                }
                var data = new Prescriptions
                {
                    RecordId = prescriptions.RecordId,
                    MedicineName = prescriptions.MedicineName,
                    Instructions = prescriptions.Instructions,
                    Dosage = prescriptions.Dosage
                };

                await _repository.AddPrescriptionAsync(data);

                _logger.LogInformation("Prescription added successfully for Record ID: {RecordId}", prescriptions.RecordId);
                return (true, "Prescription Added Sucessfully");
            }
            catch (MedicalRecordNotFoundException ex)
            {
                _logger.LogWarning(ex, "Fetch failed: {Message}", ex.Message);
                return (false, ex.Message);
            }
            
        }

        public async Task<(bool sucess, string message, PrescriptionResponceDto? data)> GetPrescriptionById(int id)
        {
            _logger.LogInformation("Fetching prescription for ID: {Id}", id);
            try
            {
                var result=await _repository.GetPrescriptionByIdAsync(id);
                if (result==null)
                {
                    throw new PerscriptionNotFoundException($"Prescription with ID {id} not found.");
                }

                _logger.LogInformation("Successfully retrieved prescription for ID: {Id}",id);
                return (true,"Prescription retrieved successfully.",ToPrescriptionResponce(result));
            }
            catch (PerscriptionNotFoundException ex)
            {
                _logger.LogWarning(ex,"Fetch failed: {Message}",ex.Message);
                return (false,ex.Message,null);
            }
        }



        public async Task<(bool sucess, string message, List<PrescriptionResponceDto>? data)> GetPrescriptionsByRecordId(int recordId)
        {
            _logger.LogInformation("Fetching prescriptions for Record ID: {RecordId}",recordId);

            try
            {
                var result=await _repository.GetPrescriptionByRecordIdAsync(recordId);

                if (result==null || !result.Any())
                {
                    throw new NoPerscriptionFoundException($"No prescriptions found for Record ID {recordId}.");
                }

                var data=result.Select(ToPrescriptionResponce).ToList();

                _logger.LogInformation("Successfully retrieved {Count} prescriptions for Record ID: {RecordId}",data.Count,recordId);
                return (true, "Prescriptions retrieved successfully.",data);
            }
            catch (NoPerscriptionFoundException ex)
            {
                _logger.LogWarning(ex, "Fetch failed: {Message}",ex.Message);
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool sucess, string message, PrescriptionResponceDto? data)> UpdatePrescription(PrescriptionUpdateDto prescriptions)
        {
            _logger.LogInformation("Attempting to update prescription with ID: {Id}",prescriptions.PrescriptionId);

            try
            {
                var result=await _repository.GetPrescriptionByIdAsync(prescriptions.PrescriptionId);

                if (result==null)
                {
                    throw new PerscriptionNotFoundException($"Prescription with ID {prescriptions.PrescriptionId} not found.");
                }

                result.Instructions=prescriptions.Instructions;
                result.MedicineName=prescriptions.MedicineName;
                result.Dosage=prescriptions.Dosage;

                await _repository.UpdatePrescriptionAsync(result);

                _logger.LogInformation("Prescription with ID {Id} updated successfully.",prescriptions.PrescriptionId);
                return (true, "Prescription updated successfully.",ToPrescriptionResponce(result));
            }
            catch (PerscriptionNotFoundException ex)
            {
                _logger.LogWarning("Update failed: {Message}",ex.Message);
                return (false,ex.Message,null);
            }
        }

        private static PrescriptionResponceDto ToPrescriptionResponce(Prescriptions prescriptions)
        {
            return new PrescriptionResponceDto
            {
                AppointmentId=prescriptions.MedicalRecord!.AppointmentId,
                PrescriptionId=prescriptions.PrescriptionId,
                Dosage=prescriptions.Dosage,
                Instructions=prescriptions.Instructions,
                MedicineName=prescriptions.MedicineName
            };

        }
    }
}