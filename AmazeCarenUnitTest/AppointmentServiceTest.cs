using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazeCareWebApi.Dtos.AppointmentDtos;
using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.PatientDtos;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Implementation;
using AmazeCareWebApi.Services.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace AmazeCareWebApi.Tests.Services
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private Mock<IPatientService> _patientServiceMock;
        private Mock<IDoctorService> _doctorServiceMock;
        private Mock<ILogger<AppointmentService>> _loggerMock;
        private AppointmentService _appointmentService;

        [SetUp]
        public void Setup()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _patientServiceMock = new Mock<IPatientService>();
            _doctorServiceMock = new Mock<IDoctorService>();
            _loggerMock = new Mock<ILogger<AppointmentService>>();

            _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object, _patientServiceMock.Object, _doctorServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task AddAppointment_WhenValidData_ShouldReturnSuccessMessage()
        {
            var createDto = new AppointmentCreateDto
            {
                PatientId = 1,
                DoctorId = 1,
                AppointmentDate = new DateOnly(2025, 1, 1),
                PreferedTime = new TimeOnly(10, 0),
                NatureOfVisit = "Checkup",
                Status = "Requested",
                SymptomsDescription = "Fever"
            };

            var patientData = new PatientResponceDto { PatientId = 1 };

            _patientServiceMock.Setup(s => s.GetPatientById(1))
                .ReturnsAsync(("Found", patientData));

            var doctorData = new DoctorResponceDto { DoctorId = 1 };

            _doctorServiceMock.Setup(s => s.GetDoctorByIdAsync(1))
                .ReturnsAsync(("Found", doctorData));

            _appointmentRepositoryMock.Setup(repo => repo.AddAppointmentRecordsAsync(It.IsAny<Appointment>()));

            var result = await _appointmentService.AddAppointment(createDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Appointment Added Successfully."));
            _appointmentRepositoryMock.Verify(repo => repo.AddAppointmentRecordsAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Test]
        public async Task AddAppointment_WhenPatientNotFound_ShouldReturnErrorMessage()
        {
            var createDto = new AppointmentCreateDto { PatientId = 99, DoctorId = 1 };

            _patientServiceMock.Setup(s => s.GetPatientById(99))
                .ReturnsAsync(("Not Found", (PatientResponceDto?)null));

            var result = await _appointmentService.AddAppointment(createDto);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("not found").IgnoreCase);
            _appointmentRepositoryMock.Verify(repo => repo.AddAppointmentRecordsAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Test]
        public async Task AddAppointment_WhenDoctorNotFound_ShouldReturnErrorMessage()
        {
            var createDto = new AppointmentCreateDto { PatientId = 1, DoctorId = 99 };

            var patientData = new PatientResponceDto { PatientId = 1 };
            _patientServiceMock.Setup(s => s.GetPatientById(1))
                .ReturnsAsync(("Found", patientData));

            _doctorServiceMock.Setup(s => s.GetDoctorByIdAsync(99))
                .ReturnsAsync(("Not Found", (DoctorResponceDto?)null));

            var result = await _appointmentService.AddAppointment(createDto);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("not found").IgnoreCase);
            _appointmentRepositoryMock.Verify(repo => repo.AddAppointmentRecordsAsync(It.IsAny<Appointment>()), Times.Never);
        }

        [Test]
        public async Task GetAppointmentById_WhenExists_ShouldReturnData()
        {
            var appointment = new Appointment { AppointmentId = 1, Status = "Upcoming" };

            _appointmentRepositoryMock.Setup(repo => repo.GetAppointmentByIdAsync(1))
                .ReturnsAsync(appointment);

            var result = await _appointmentService.GetAppointmentById(1);

            Assert.That(result.Success, Is.True);
            Assert.That(result.data, Is.Not.Null);
            Assert.That(result.data.AppointmentId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAppointmentById_WhenDoesNotExist_ShouldReturnErrorMessage()
        {
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointmentByIdAsync(99))
                .ReturnsAsync((Appointment?)null);

            var result = await _appointmentService.GetAppointmentById(99);

            Assert.That(result.Success, Is.False);
            Assert.That(result.data, Is.Null);
        }

        [Test]
        public async Task RemoveAppointment_WhenExists_ShouldReturnSuccess()
        {
            _appointmentRepositoryMock.Setup(repo => repo.DeleteAppointmentAsync(1))
                .ReturnsAsync(true);

            var result = await _appointmentService.RemoveAppointment(1);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Appointment Removed"));
        }

        [Test]
        public async Task RemoveAppointment_WhenDoesNotExist_ShouldReturnErrorMessage()
        {
            _appointmentRepositoryMock.Setup(repo => repo.DeleteAppointmentAsync(99))
                .ReturnsAsync(false);

            var result = await _appointmentService.RemoveAppointment(99);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Does.Contain("not found").IgnoreCase);
        }





        [Test]
        public async Task UpdateAppointmentStatus_WhenExists_ShouldReturnSuccessAndVerifyUpdate()
        {
            var appointment = new Appointment { AppointmentId = 1, Status = "Requested" };
            var updateDto = new AppointmentUpdateStatusDto { AppointmentId = 1, Status = "Confirmed" };

            _appointmentRepositoryMock.Setup(repo => repo.GetAppointmentByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock.Setup(repo => repo.UpdateAppointment(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            var result = await _appointmentService.UpdateAppointmentStatus(updateDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.data, Is.Not.Null);
            Assert.That(result.data.Status, Is.EqualTo("Confirmed"));
            _appointmentRepositoryMock.Verify(repo => repo.UpdateAppointment(It.Is<Appointment>(a => a.Status == "Confirmed")), Times.Once);
        }

        [Test]
        public async Task RescheduleAppointment_WhenExists_ShouldReturnSuccessAndVerifyUpdate()
        {
            var appointment = new Appointment
            {
                AppointmentId = 1,
                Status = "Requested",
                AppointmentDate = new DateOnly(2025, 1, 1),
                PreferedTime = new TimeOnly(10, 0)
            };

            var newSlot = new DateTime(2025, 2, 2, 14, 30, 0);
            var rescheduleDto = new AppointmentRescheduleDto
            {
                AppointmentId = 1,
                RescheduledSlot = newSlot
            };

            _appointmentRepositoryMock.Setup(repo => repo.GetAppointmentByIdAsync(1))
                .ReturnsAsync(appointment);

            _appointmentRepositoryMock.Setup(repo => repo.UpdateAppointment(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            var result = await _appointmentService.RescheduleAppointment(rescheduleDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.data, Is.Not.Null);
            Assert.That(result.data.Status, Is.EqualTo("Rescheduled"));
            _appointmentRepositoryMock.Verify(repo => repo.UpdateAppointment(It.IsAny<Appointment>()), Times.Once);
        }
    }
}
