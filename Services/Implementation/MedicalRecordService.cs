using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.MedicalRecordDtos;
using AmazeCareWebApi.Exceptions.AppointmentException;
using AmazeCareWebApi.Exceptions.MedicalRecordException;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Implementation;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Interface;

namespace AmazeCareWebApi.Services.Implementation
{
    public class MedicalRecordService:IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _repository;
        private readonly ILogger<MedicalRecordService> _logger;
        private readonly IAppotimentService _appotimentService;
        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository, ILogger<MedicalRecordService> logger, IAppotimentService appotimentService)
        {
            _repository = medicalRecordRepository;
            _logger = logger;
            _appotimentService = appotimentService;
        }

        public async Task<(bool Success, string Message)> AddMedicalRecordAsync(MedicalRecordCreateDto medicalRecordCreate)
        {
            _logger.LogInformation("Attempting to add a new medical record for Appointment ID: {AppointmentId}", medicalRecordCreate.AppointmentId);
            try
            {
                var appointment = await _appotimentService.GetAppointmentById(medicalRecordCreate.AppointmentId);
                if (appointment.data == null)
                {
                    throw new AppointmentNotFoundException($"No appointmnet found for {medicalRecordCreate.AppointmentId}");
                }
                var data = new MedicalRecords
                {
                    AppointmentId = medicalRecordCreate.AppointmentId,
                    CurrentSymptoms = medicalRecordCreate.CurrentSymptoms,
                    PhysicalExamination = medicalRecordCreate.PhysicalExamination,
                    RecommendedTests = medicalRecordCreate.MedicalTest,
                    TreatmentPlan = medicalRecordCreate.TreatmentPlan
                };

                await _repository.AddMedicalRecordsAsync(data);
            }
            catch (AppointmentNotFoundException ex)
            {
                _logger.LogWarning(ex, "add failed: {Message}", ex.Message);
                return (false, ex.Message);
            }

            _logger.LogInformation("Medical record added successfully for Appointment ID: {AppointmentId}",medicalRecordCreate.AppointmentId);
            return (true, "Record Added Sucessfully.");
        }

        public async Task<(bool Success, string Message, MedicalRecordResponceDto? Data)> GetMedicalRecordByIdAsync(int recordId)
        {
            _logger.LogInformation("Fetching medical record for ID: {RecordId}", recordId);

            try
            {
                var result = await _repository.GetMedicalRecordByIdAsync(recordId);

                if (result == null)
                {
                    throw new MedicalRecordNotFoundException($"Medical record with ID {recordId} was not found.");
                }

                _logger.LogInformation("Successfully retrieved medical record for ID: {RecordId}", recordId);
                return (true,"Medical record retrieved successfully.",ToMedicalResponceDto(result));
            }
            catch (MedicalRecordNotFoundException ex)
            {
                _logger.LogWarning(ex, "Fetch failed: {Message}", ex.Message);
                return (false, ex.Message, null);
            }
        }

        public async Task<(bool Success, string Message, MedicalRecordResponceDto? Data)> UpdateMedicalRecord(MedicalRecordUpdateDto updateDto)
        {
            _logger.LogInformation("Attempting to update medical record with ID: {RecordId}", updateDto.RecordId);

            try
            {
               
                var result = await _repository.GetMedicalRecordByIdAsync(updateDto.RecordId);

                if (result == null)
                {
                    throw new MedicalRecordNotFoundException($"Medical record with ID {updateDto.RecordId} was not found.");
                }

                result.CurrentSymptoms = updateDto.CurrentSymptoms;
                result.PhysicalExamination = updateDto.PhysicalExamination;
                result.TreatmentPlan = updateDto.TreatmentPlan;
                result.RecommendedTests = updateDto.MedicalTest;

                await _repository.UpdateMedicalRecordAsync(result);

                _logger.LogInformation("Medical record with ID {RecordId} updated successfully.", updateDto.RecordId);
                return (true,"Medical record updated successfully.",ToMedicalResponceDto(result));
            }
            catch (MedicalRecordNotFoundException ex)
            {
                _logger.LogWarning(ex,"Update failed: {Message}",ex.Message);
                return (false, ex.Message, null);
            }
        }

        private MedicalRecordResponceDto ToMedicalResponceDto(MedicalRecords medicalRecords)
        {
            return new MedicalRecordResponceDto
            {
                RecordId=medicalRecords.RecordId,
                AppointmentDate=medicalRecords.Appointment?.AppointmentDate ?? default,
                AppointmentId=medicalRecords.Appointment?.AppointmentId ?? 0,
                DoctorId=medicalRecords.Appointment?.DoctorId ?? 0,
                DoctorName=medicalRecords.Appointment?.Doctor?.DoctorName, 
                PatientId=medicalRecords.Appointment?.PatientId ?? 0,
                PatientName=medicalRecords.Appointment?.Patient?.FullName,
                CurrentSymptoms=medicalRecords.CurrentSymptoms,
                MedicalTest=medicalRecords.RecommendedTests,
                PhysicalExamination=medicalRecords.PhysicalExamination,
                TreatmentPlan=medicalRecords.TreatmentPlan,
                Prescriptions=medicalRecords.Prescriptions?
                    .Select(p=> new ReportPerscriptionDTo
                    {
                        MedicineName=p.MedicineName??"Not assigned",
                        Dosage = p.Dosage??"Not assigned",
                        Instructions = p.Instructions??"Not assigned"
                    }).ToList()??new List<ReportPerscriptionDTo>()
            };
        }
    }
}
