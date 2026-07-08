using AmazeCareWebApi.Dtos.PatientDtos;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Implementation;
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
    public class PatientServiceTest
    {
        private Mock<IPatientRepository> _patientRepositoryMock;
        private Mock<ILogger<PatientService>> _loggerMock;
        private PatientService _patientService;

        [SetUp]
        public void Setup()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _loggerMock = new Mock<ILogger<PatientService>>();
            _patientService = new PatientService(_patientRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task AddPatient_ShouldReturnSuccess_WhenValidDataProvided()
        {
            var createDto = new PatientCreateDto
            {
                FullName = "Poki",
                ContactNumber = "87398221",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male"
            };
            

            _patientRepositoryMock.Setup(repo => repo.AddPatientAsync(It.IsAny<Patient>()));

            var result = await _patientService.AddPatient(createDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Patient Added Successfully."));

            _patientRepositoryMock.Verify(repo => repo.AddPatientAsync(It.IsAny<Patient>()), Times.Once);
        }


        [Test]
        public async Task GetPatientById_WhenPatientExists_ShouldReturnPatientData()
        {
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "Poki",
                ContactNumber = "87398221",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male"
            };

            _patientRepositoryMock.Setup(repo => repo.GetPatientByIdAsync(1))
                .ReturnsAsync(patient);

            var result = await _patientService.GetPatientById(1);

            Assert.That(result.Message, Is.EqualTo("Patient Found"));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.FullName, Is.EqualTo("Poki"));
            Assert.That(result.Data.ContactNumber, Is.EqualTo("87398221"));
            Assert.That(result.Data.Gender, Is.EqualTo("Male"));
            _patientRepositoryMock.Verify(repo => repo.GetPatientByIdAsync(1), Times.Once);
        }

        [TestCase(10)]
        [TestCase(11)]
        public async Task GetPatientById_WhenPatientNotExists_ShouldReturnNotPatientData(int id)
        {
            _patientRepositoryMock.Setup(repo => repo.GetPatientByIdAsync(id))
                .ReturnsAsync((Patient?)null);

            var result = await _patientService.GetPatientById(id);

            Assert.That(result.Message, Is.EqualTo($"Patient record {id} not found."));
            Assert.That(result.Data, Is.Null);
            _patientRepositoryMock.Verify(repo => repo.GetPatientByIdAsync(id), Times.Once);

        }

        [Test]      
        public async Task DeletePatientById_WhenPatientExists_ShouldReturnSuccessMessage()
        {
            var patient = new Patient
            {
                PatientId = 1,
                FullName = "Poki",
                ContactNumber = "87398221",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male"
            };

            _patientRepositoryMock.Setup(repo => repo.GetPatientByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock.Setup(repo => repo.DeletePatientByIdAsync(1));

            var result = await _patientService.DeletePatientById(1);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Patient Record Deleted Successfully."));
            _patientRepositoryMock.Verify(repo => repo.DeletePatientByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task DeletePatientById_WhenPatientDoesNotExist_ShouldReturnNotFoundMessage()
        {
            _patientRepositoryMock.Setup(repo => repo.GetPatientByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            var result = await _patientService.DeletePatientById(99);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Patient record with ID 99 not found."));

            _patientRepositoryMock.Verify(repo => repo.DeletePatientByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task UpdatePatient_WhenPatientExists_ShouldReturnSuccessMessage()
        {
            var existingPatient = new Patient { 
                PatientId = 1, 
                FullName = "POki",
                ContactNumber="71298731",
                DateOfBirth= new DateOnly(1990, 1, 1),
                Gender="Male"
            };

            var updateDto = new PatientUpdateDto
            {
                PatientId = 1,
                FullName = "pokemon",
                ContactNumber = "5433213",
                DateOfBirth = new DateOnly(1995, 5, 5),
                Gender = "Male"
            };

            _patientRepositoryMock.Setup(repo => repo.GetPatientByIdAsync(1))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock.Setup(repo => repo.UpdatePatientAsync(It.IsAny<Patient>()));

            var result = await _patientService.UpdatePatient(updateDto);

            Assert.That(result.Message, Is.EqualTo("Updated Patient Sucessfully"));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.FullName, Is.EqualTo("pokemon"));
            Assert.That(result.Data.ContactNumber, Is.EqualTo("5433213"));
        }

        [Test]
        public async Task UpdatePatient_WhenPatientDoesNotExist_ShouldReturnNotFoundMessage()
        {
            var updateDto = new PatientUpdateDto { PatientId = 99, FullName = "New Name" };

            _patientRepositoryMock.Setup(repo => repo.GetPatientByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            var result = await _patientService.UpdatePatient(updateDto);

            Assert.That(result.Message, Is.EqualTo("Patient record 99 not found."));
            Assert.That(result.Data, Is.Null);

            _patientRepositoryMock.Verify(repo => repo.UpdatePatientAsync(It.IsAny<Patient>()), Times.Never);
        }


        [Test]
        public async Task GetAllPatients_WhenCalled_ShouldReturnListOfPatients()
        {
            var patientsList = new List<Patient>
            {
                new Patient { PatientId = 1, FullName = "Patient One" },
                new Patient { PatientId = 2, FullName = "Patient Two" }
            };

            _patientRepositoryMock.Setup(repo => repo.GetAllPatientsAsync())
                .ReturnsAsync(patientsList);

            var result = await _patientService.GetAllPatients();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FullName, Is.EqualTo("Patient One"));
            Assert.That(result[1].FullName, Is.EqualTo("Patient Two"));
            _patientRepositoryMock.Verify(repo => repo.GetAllPatientsAsync(), Times.Once);
        }

    }
}
