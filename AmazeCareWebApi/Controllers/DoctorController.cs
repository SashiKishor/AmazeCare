using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.PatientDtos;
using AmazeCareWebApi.Services.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCareWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;
        private readonly IValidator<DoctorCreateDto> _validator;
        private readonly IValidator<DoctorUpdateDto> _updateValidator;

        public DoctorController(IDoctorService service, IValidator<DoctorCreateDto> validator, IValidator<DoctorUpdateDto> updateValidator)
        {
            _service = service;
            _validator = validator;
            _updateValidator= updateValidator;
        }

        [HttpGet("/GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var result = await _service.GetAllDoctorsAsync();
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "All Doctors Data Retrived",
                Data = result
            });
        }

        [Authorize(Roles = "Admin, Patient, Doctor")]
        [HttpGet("DoctorBy{id}")]
        public async Task<IActionResult> GetDoctorById(int id) {
            var result=await _service.GetDoctorByIdAsync(id);
            if (result.Data == null) {
                return NotFound(new {
                    StatusCode=StatusCodes.Status404NotFound,
                    Message=result.Message
                });
            }
            return Ok(new
            {
                StatusCode=StatusCodes.Status200OK,
                Message= result.Message,
                Data=result.Data
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("/AddDoctor")]
        public async Task<IActionResult> AddDoctor(DoctorCreateDto doctorCreateDto)
        {
            var valid = await _validator.ValidateAsync(doctorCreateDto);

            if (!valid.IsValid)
            {

                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await _service.AddDoctorAsync(doctorCreateDto);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data = result.Data
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status201Created,
                Message = result.Message,
                Data = result.Data
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("/RemoveDoctorBy{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _service.DeleteDoctorByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status201Created,
                Message = result.Message
            });
        }

        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctorExperience(DoctorUpdateDto doctorUpdateDto)
        {
            var valid = await _updateValidator.ValidateAsync(doctorUpdateDto);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result=await _service.UpdateDoctorDetailsAsync(doctorUpdateDto);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = result.Message,
                    Data=result.data
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status201Created,
                Message = result.Message,
                Data = result.data
            });
        }

        [HttpGet("/availableDoctors")]
        public async Task<IActionResult> AvailableDoctors([FromQuery] DoctorAvailabiltyRequest doctorAvailabiltyRequest)
        {
            var result = await _service.GetAvailableDoctorsAsync(doctorAvailabiltyRequest);
            if (!result.Sucess)
            {
                return NoContent();
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status201Created,
                Message = result.Message,
                Data = result.data
            });
        }



    }
}
