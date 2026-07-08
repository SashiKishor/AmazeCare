using AmazeCareWebApi.Dtos.DoctorDtos;
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
    public class DoctorServiceTest
    {
        private Mock<IDoctorRepository> _repository;
        private Mock<ILogger<DoctorService>> _logger;
        private DoctorService _service;

        [SetUp]
        public void setup()
        {
            _repository = new Mock<IDoctorRepository>();
            _logger = new Mock<ILogger<DoctorService>>();
            _service = new DoctorService(_repository.Object, _logger.Object);
        }
        [Test]
        public async Task AddDoctorAsync_WhenValidDataProvided_ShouldReturnSuccessMessage()
        {
            var createDto = new DoctorCreateDto
            {
                DoctorName = "Dr.Smith",
                Designation = "Senior Surgeon",
                Experience = 15,
                Qualification = "MBBS, MD",
                Speciality = "Cardiology"
            };

            _repository.Setup(repo => repo.AddDoctorAsync(It.IsAny<Doctor>()));

            var result = await _service.AddDoctorAsync(createDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Doctor record added."));
            _repository.Verify(a => a.AddDoctorAsync(It.IsAny<Doctor>()), Times.Once);
        }

        [Test]
        public async Task GetDoctorByIdAsync_WhenDoctorExists_ShouldReturnDoctorData()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Dr. Smith",
                Designation = "Senior Surgeon",
                Experience = 15,
                Qualification = "MBBS, MD",
                Speciality = "Cardiology"
            };

            _repository.Setup(repo => repo.GetDoctorByIdAsync(1))
                .ReturnsAsync(doctor);

            var result = await _service.GetDoctorByIdAsync(1);

            Assert.That(result.Message, Is.EqualTo("Doctor Found"));
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.DoctorName, Is.EqualTo("Dr. Smith"));
            Assert.That(result.Data.Designation, Is.EqualTo("Senior Surgeon"));
            Assert.That(result.Data.Speciality, Is.EqualTo("Cardiology"));
            _repository.Verify(repo => repo.GetDoctorByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task GetDoctorByIdAsync_WhenDoctorDoesNotExist_ShouldReturnNotFoundMessage()
        {
            _repository.Setup(repo => repo.GetDoctorByIdAsync(2))
                .ReturnsAsync((Doctor?)null);

            var result = await _service.GetDoctorByIdAsync(2);

            Assert.That(result.Message, Is.EqualTo("Doctor record 2 not found."));
            Assert.That(result.Data, Is.Null);
            _repository.Verify(repo => repo.GetDoctorByIdAsync(2), Times.Once);
        }

        [Test]
        public async Task DeleteDoctorByIdAsync_WhenDoctorExists_ShouldReturnSuccessMessage()
        {
            var doctor = new Doctor
            {
                DoctorId = 1,
                DoctorName = "Dr. Smith",
                Designation = "Senior Surgeon",
                Experience = 15,
                Qualification = "MBBS, MD",
                Speciality = "Cardiology"
            };

            _repository.Setup(repo => repo.GetDoctorByIdAsync(1))
                .ReturnsAsync(doctor);
            _repository.Setup(repo => repo.DeleteDoctorByIdAsync(1));

            var result = await _service.DeleteDoctorByIdAsync(1);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Doctor data deleted."));
            _repository.Verify(repo => repo.DeleteDoctorByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task DeleteDoctorByIdAsync_WhenDoctorDoesNotExist_ShouldReturnNotFoundMessage()
        {
            _repository.Setup(repo => repo.GetDoctorByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            var result = await _service.DeleteDoctorByIdAsync(99);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Doctor record 99 not found."));
            _repository.Verify(repo => repo.DeleteDoctorByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task UpdateDoctorDetailsAsync_WhenDoctorExists_ShouldReturnSuccessMessage()
        {
            var existingDoctor = new Doctor {
                DoctorId = 1,
                DoctorName = "Dr. Smith",
                Designation = "Senior Surgeon",
                Experience = 15,
                Qualification = "MBBS, MD",
                Speciality = "Cardiology"
            };

            var updateDto = new DoctorUpdateDto
            {
                DoctorId = 1,
                DoctorName = "pokemon",
                Speciality = "Cardiology"
            };

            _repository.Setup(repo => repo.GetDoctorByIdAsync(1))
                .ReturnsAsync(existingDoctor);
            _repository.Setup(repo => repo.UpdateDoctorRecordAsync(It.IsAny<Doctor>()));

            var result = await _service.UpdateDoctorDetailsAsync(updateDto);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Doctor Updated Successfully"));
            Assert.That(result.data, Is.Not.Null);
            Assert.That(result.data.DoctorName, Is.EqualTo("pokemon"));

            _repository.Verify(repo => repo.UpdateDoctorRecordAsync(It.IsAny<Doctor>()), Times.Once);
        }

        [Test]
        public async Task UpdateDoctorDetailsAsync_WhenDoctorDoesNotExist_ShouldReturnNotFoundMessage()
        {
            var updateDto = new DoctorUpdateDto { DoctorId = 99, DoctorName = "pokemon" };

            _repository.Setup(repo => repo.GetDoctorByIdAsync(99))
                .ReturnsAsync((Doctor?)null);

            var result = await _service.UpdateDoctorDetailsAsync(updateDto);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Doctor record 99 not found."));
            Assert.That(result.data, Is.Null);
            _repository.Verify(repo => repo.GetDoctorByIdAsync(99), Times.Once);
            _repository.Verify(repo => repo.UpdateDoctorRecordAsync(It.IsAny<Doctor>()), Times.Never);
        }

        [Test]
        public async Task GetAllDoctorsAsync_WhenCalled_ShouldReturnListOfDoctors()
        {
            var doctorsList = new List<Doctor>
            {
                new Doctor { DoctorId = 1, DoctorName = "Dr. One" },
                new Doctor { DoctorId = 2, DoctorName = "Dr. Two" }
            };

            _repository.Setup(repo => repo.GetAllDoctorsAsync())
                .ReturnsAsync(doctorsList);

            var result = await _service.GetAllDoctorsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].DoctorName, Is.EqualTo("Dr. One"));
            _repository.Verify(repo => repo.GetAllDoctorsAsync(), Times.Once);
        }


   
    }
}
