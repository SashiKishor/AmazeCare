using AmazeCareWebApi.Dtos.AppointmentDtos;
using AmazeCareWebApi.Dtos.MedicalRecordDtos;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Implementation;
using AmazeCareWebApi.Services.Interface;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazeCarenUnitTest
{
    [TestFixture]
    public class MediacalRecordServiceTest
    {
        private Mock<IMedicalRecordRepository> _medicalRecordRepositoryMock;
        private Mock<ILogger<MedicalRecordService>> _loggerMock;
        private Mock<IAppotimentService> _appointmentServiceMock;
        private MedicalRecordService _medicalRecordService;

        [SetUp]
        public void Setup()
        {
            _medicalRecordRepositoryMock = new Mock<IMedicalRecordRepository>();
            _loggerMock = new Mock<ILogger<MedicalRecordService>>();
            _appointmentServiceMock = new Mock<IAppotimentService>();

            _medicalRecordService = new MedicalRecordService(_medicalRecordRepositoryMock.Object, _loggerMock.Object, _appointmentServiceMock.Object);
        }

        [Test]
        public async Task AddMedicalRecordAsync_WhenValidDataProvided_ShouldReturnSuccessMessage()
        {
            var createDto = new MedicalRecordCreateDto
            {
                AppointmentId = 1,
                CurrentSymptoms = "Fever and Cough",
                PhysicalExamination = "Normal BP",
                MedicalTest = "Blood Test",
                TreatmentPlan = "Rest and hydration"
            };

            var appointmentData = new AppointmentResponceDto
            {
                AppointmentId = 1,
                DoctorId = 1,
                PatientName = "sashi",
                NatureOfVisit = "General"
            };

            _appointmentServiceMock.Setup(s => s.GetAppointmentById(1))
                .ReturnsAsync((true, "Appointment Found", appointmentData));

            _medicalRecordRepositoryMock.Setup(repo => repo.AddMedicalRecordsAsync(It.IsAny<MedicalRecords>()));

            var result = await _medicalRecordService.AddMedicalRecordAsync(createDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Record Added Sucessfully."));

            _appointmentServiceMock.Verify(s => s.GetAppointmentById(1), Times.Once);
            _medicalRecordRepositoryMock.Verify(repo => repo.AddMedicalRecordsAsync(It.IsAny<MedicalRecords>()), Times.Once);
        }

        [Test]
        public async Task AddMedicalRecordAsync_WhenAppointmentNotFound_ShouldReturnErrorMessage()
        {
            var createDto = new MedicalRecordCreateDto { AppointmentId = 99 };

            _appointmentServiceMock.Setup(s => s.GetAppointmentById(99))
                .ReturnsAsync((false, "Not Found", (AppointmentResponceDto?)null));

            var result = await _medicalRecordService.AddMedicalRecordAsync(createDto);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("No appointmnet found").IgnoreCase);

            _appointmentServiceMock.Verify(repo => repo.GetAppointmentById(99), Times.Once);
            _medicalRecordRepositoryMock.Verify(repo => repo.AddMedicalRecordsAsync(It.IsAny<MedicalRecords>()), Times.Never);
        }

        [Test]
        public async Task GetMedicalRecordByIdAsync_WhenRecordExists_ShouldReturnRecordData()
        {
            var existingRecord = new MedicalRecords
            {
                RecordId = 1,
                AppointmentId = 1,
                CurrentSymptoms = "Fever",
                TreatmentPlan = "Rest"
            };

            _medicalRecordRepositoryMock.Setup(repo => repo.GetMedicalRecordByIdAsync(1))
                .ReturnsAsync(existingRecord);

            var result = await _medicalRecordService.GetMedicalRecordByIdAsync(1);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Medical record retrieved successfully."));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.RecordId, Is.EqualTo(1));
            Assert.That(result.Data.CurrentSymptoms, Is.EqualTo("Fever"));

            _medicalRecordRepositoryMock.Verify(repo => repo.GetMedicalRecordByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task GetMedicalRecordByIdAsync_WhenRecordDoesNotExist_ShouldReturnErrorMessage()
        {
            _medicalRecordRepositoryMock.Setup(repo => repo.GetMedicalRecordByIdAsync(99))
                .ReturnsAsync((MedicalRecords?)null);

            var result = await _medicalRecordService.GetMedicalRecordByIdAsync(99);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("not found").IgnoreCase);
            Assert.That(result.Data, Is.Null);

            _medicalRecordRepositoryMock.Verify(repo => repo.GetMedicalRecordByIdAsync(99), Times.Once);
        }

        [Test]
        public async Task UpdateMedicalRecord_WhenRecordExists_ShouldReturnSuccessMessage()
        {
            var existingRecord = new MedicalRecords
            {
                RecordId = 1,
                CurrentSymptoms = "head aceh"
            };

            var updateDto = new MedicalRecordUpdateDto
            {
                RecordId = 1,
                CurrentSymptoms = "puke",
                PhysicalExamination = "weak",
                MedicalTest = "appendix",
                TreatmentPlan = "operation"
            };

            _medicalRecordRepositoryMock.Setup(repo => repo.GetMedicalRecordByIdAsync(1))
                .ReturnsAsync(existingRecord);

            _medicalRecordRepositoryMock.Setup(repo => repo.UpdateMedicalRecordAsync(It.IsAny<MedicalRecords>()));

            var result = await _medicalRecordService.UpdateMedicalRecord(updateDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Medical record updated successfully."));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.CurrentSymptoms, Is.EqualTo("puke"));
            Assert.That(result.Data.PhysicalExamination, Is.EqualTo("weak"));
            Assert.That(result.Data.MedicalTest, Is.EqualTo("appendix"));
            Assert.That(result.Data.TreatmentPlan, Is.EqualTo("operation"));

            _medicalRecordRepositoryMock.Verify(repo => repo.UpdateMedicalRecordAsync(It.IsAny<MedicalRecords>()), Times.Once);
        }

        [Test]
        public async Task UpdateMedicalRecord_WhenRecordDoesNotExist_ShouldReturnErrorMessage()
        {
            var updateDto = new MedicalRecordUpdateDto { RecordId = 99 };

            _medicalRecordRepositoryMock.Setup(repo => repo.GetMedicalRecordByIdAsync(99))
                .ReturnsAsync((MedicalRecords?)null);

            var result = await _medicalRecordService.UpdateMedicalRecord(updateDto);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("not found").IgnoreCase);
            Assert.That(result.Data, Is.Null);

            _medicalRecordRepositoryMock.Verify(repo => repo.UpdateMedicalRecordAsync(It.IsAny<MedicalRecords>()), Times.Never);
        }
    }
}
