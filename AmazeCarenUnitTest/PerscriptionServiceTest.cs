using AmazeCareWebApi.Dtos.MedicalRecordDtos;
using AmazeCareWebApi.Dtos.PrescriptionDtos;
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
    public class PerscriptionServiceTest
    {
        private Mock<IPrescriptionRepository> _prescriptionRepositoryMock;
        private Mock<ILogger<PescriptionService>> _loggerMock;
        private Mock<IMedicalRecordService> _medicalRecordServiceMock;
        private PescriptionService _prescriptionService;

        [SetUp]
        public void Setup()
        {
            _prescriptionRepositoryMock = new Mock<IPrescriptionRepository>();
            _loggerMock = new Mock<ILogger<PescriptionService>>();
            _medicalRecordServiceMock = new Mock<IMedicalRecordService>();

            _prescriptionService = new PescriptionService(_prescriptionRepositoryMock.Object, _loggerMock.Object, _medicalRecordServiceMock.Object);
        }

        [Test]
        public async Task AddPrescription_WhenMedicalRecordExists_ShouldReturnSuccessMessage()
        {
            var createDto = new PrescriptionCreateDto
            {
                RecordId = 1,
                MedicineName = "Paracetamol",
                Dosage = "500mg",
                Instructions = "Twice a day"
            };

            var medicalRecordData = new MedicalRecordResponceDto { RecordId = 1 };

            _medicalRecordServiceMock.Setup(s => s.GetMedicalRecordByIdAsync(1))
                .ReturnsAsync((true, "Record Found", medicalRecordData));

            _prescriptionRepositoryMock.Setup(repo => repo.AddPrescriptionAsync(It.IsAny<Prescriptions>()));

            var result = await _prescriptionService.AddPrescription(createDto);

            Assert.That(result.sucess, Is.True);
            Assert.That(result.message, Is.EqualTo("Prescription Added Sucessfully"));

            _prescriptionRepositoryMock.Verify(repo => repo.AddPrescriptionAsync(It.IsAny<Prescriptions>()), Times.Once);
        }

        [Test]
        public async Task AddPrescription_WhenMedicalRecordDoesNotExist_ShouldReturnErrorMessage()
        {
            var createDto = new PrescriptionCreateDto { RecordId = 99 };

            _medicalRecordServiceMock.Setup(s => s.GetMedicalRecordByIdAsync(99))
                .ReturnsAsync((false, "Not Found", (MedicalRecordResponceDto?)null));

            var result = await _prescriptionService.AddPrescription(createDto);

            Assert.That(result.sucess, Is.False);
            Assert.That(result.message, Does.Contain("No medical record found").IgnoreCase);

            _prescriptionRepositoryMock.Verify(repo => repo.AddPrescriptionAsync(It.IsAny<Prescriptions>()), Times.Never);
        }

        [Test]
        public async Task GetPrescriptionById_WhenPrescriptionExists_ShouldReturnPrescriptionData()
        {
            var existingPrescription = new Prescriptions
            {
                PrescriptionId = 1,
                MedicineName = "Amoxicillin",
                Dosage = "250mg",
                Instructions = "After meals",
                MedicalRecord = new MedicalRecords { AppointmentId = 10 }
            };

            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionByIdAsync(1))
                .ReturnsAsync(existingPrescription);

            var result = await _prescriptionService.GetPrescriptionById(1);

            Assert.That(result.sucess, Is.True);
            Assert.That(result.message, Is.EqualTo("Prescription retrieved successfully."));
            Assert.That(result.data, Is.Not.Null);
            Assert.That(result.data.MedicineName, Is.EqualTo("Amoxicillin"));
            Assert.That(result.data.AppointmentId, Is.EqualTo(10));

            _prescriptionRepositoryMock.Verify(repo => repo.GetPrescriptionByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task GetPrescriptionById_WhenPrescriptionDoesNotExist_ShouldReturnErrorMessage()
        {
            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionByIdAsync(99))
                .ReturnsAsync((Prescriptions?)null);

            var result = await _prescriptionService.GetPrescriptionById(99);

            Assert.That(result.sucess, Is.False);
            Assert.That(result.message, Does.Contain("not found").IgnoreCase);
            Assert.That(result.data, Is.Null);

            _prescriptionRepositoryMock.Verify(repo => repo.GetPrescriptionByIdAsync(99), Times.Once);
        }

        [Test]
        public async Task GetPrescriptionsByRecordId_WhenPrescriptionsExist_ShouldReturnListOfPrescriptions()
        {
            var prescriptionsList = new List<Prescriptions>
            {
                new Prescriptions
                {
                    PrescriptionId = 1,
                    MedicineName = "Medicine A",
                    MedicalRecord = new MedicalRecords { AppointmentId = 10 }
                },
                new Prescriptions
                {
                    PrescriptionId = 2,
                    MedicineName = "Medicine B",
                    MedicalRecord = new MedicalRecords { AppointmentId = 10 }
                }
            };

            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionByRecordIdAsync(1))
                .ReturnsAsync(prescriptionsList);

            var result = await _prescriptionService.GetPrescriptionsByRecordId(1);

            Assert.That(result.sucess, Is.True);
            Assert.That(result.message, Is.EqualTo("Prescriptions retrieved successfully."));
            Assert.That(result.data, Is.Not.Null);
            Assert.That(result.data.Count, Is.EqualTo(2));
            Assert.That(result.data[0].MedicineName, Is.EqualTo("Medicine A"));

            _prescriptionRepositoryMock.Verify(repo => repo.GetPrescriptionByRecordIdAsync(1), Times.Once);
        }

        [Test]
        public async Task GetPrescriptionsByRecordId_WhenNoPrescriptionsExist_ShouldReturnErrorMessage()
        {
            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionByRecordIdAsync(99))
                .ReturnsAsync(new List<Prescriptions>());

            var result = await _prescriptionService.GetPrescriptionsByRecordId(99);

            Assert.That(result.sucess, Is.False);
            Assert.That(result.message, Does.Contain("No prescriptions found").IgnoreCase);
            Assert.That(result.data, Is.Null);

            _prescriptionRepositoryMock.Verify(repo => repo.GetPrescriptionByRecordIdAsync(99), Times.Once);
        }

        [Test]
        public async Task UpdatePrescription_WhenPrescriptionExists_ShouldReturnSuccessMessage()
        {
            var existingPrescription = new Prescriptions
            {
                PrescriptionId = 1,
                MedicineName = "gobo",
                MedicalRecord = new MedicalRecords
                {
                    AppointmentId = 10,
                    RecommendedTests = "x-ray"
                }
            };

            var updateDto = new PrescriptionUpdateDto
            {
                PrescriptionId = 1,
                MedicineName = "dolo",
                Dosage = "100mg",
                Instructions = "Once a day"
            };

            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionByIdAsync(1))
                .ReturnsAsync(existingPrescription);

            _prescriptionRepositoryMock.Setup(repo => repo.UpdatePrescriptionAsync(It.IsAny<Prescriptions>()));

            var result = await _prescriptionService.UpdatePrescription(updateDto);

            Assert.That(result.sucess, Is.True);
            Assert.That(result.message, Is.EqualTo("Prescription updated successfully."));
            Assert.That(result.data, Is.Not.Null);
            Assert.That(result.data.MedicineName, Is.EqualTo("dolo"));
            _prescriptionRepositoryMock.Verify(repo => repo.UpdatePrescriptionAsync(It.IsAny<Prescriptions>()), Times.Once);
        }

        [Test]
        public async Task UpdatePrescription_WhenPrescriptionDoesNotExist_ShouldReturnErrorMessage()
        {
            var updateDto = new PrescriptionUpdateDto { PrescriptionId = 99 };

            _prescriptionRepositoryMock.Setup(repo => repo.GetPrescriptionByIdAsync(99))
                .ReturnsAsync((Prescriptions?)null);

            var result = await _prescriptionService.UpdatePrescription(updateDto);

            Assert.That(result.sucess, Is.False);
            Assert.That(result.message, Does.Contain("not found").IgnoreCase);
            Assert.That(result.data, Is.Null);

            _prescriptionRepositoryMock.Verify(repo => repo.UpdatePrescriptionAsync(It.IsAny<Prescriptions>()), Times.Never);
        }
    }
}
