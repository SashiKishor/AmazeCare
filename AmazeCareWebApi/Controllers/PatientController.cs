using AmazeCareWebApi.Dtos.PatientDtos;
using AmazeCareWebApi.Services.Interface;
using AmazeCareWebApi.Validations.patientValidation;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCareWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IValidator<PatientCreateDto> _validator;
        private readonly IValidator<PatientUpdateDto> _updateValidator;


        public PatientController(IPatientService patientService, IValidator<PatientCreateDto> validator, IValidator<PatientUpdateDto> updateValidator)
        {
            _patientService = patientService;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var result=await _patientService.GetAllPatients();
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpGet("/PatientBy{id}")]
        public async Task<IActionResult> GetPatientById(int id) {
            var result=await _patientService.GetPatientById(id);

            if (result.Data == null) { 
                return BadRequest(new {
                    statusCode=StatusCodes.Status404NotFound,
                    Message= result.Message
                });
            }
            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data=result.Data
            });
        
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpPost("/AddPatient")]
        public async Task<IActionResult> Addpatient(PatientCreateDto patientCreateDto)
        {
            var valid = await _validator.ValidateAsync(patientCreateDto);

            if (!valid.IsValid)
            {
                
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result=await _patientService.AddPatient(patientCreateDto);
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
                StatusCode=StatusCodes.Status200OK,
                Message=result.Message,
                Data=result.Data
            });
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpDelete("RemovePatient{Id}")]
        public async Task<IActionResult> DeletePatient(int Id) { 
            var result=await _patientService.DeletePatientById(Id);
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
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message
            });
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpPut("/UpdatePatient")]
        public async Task<IActionResult> UpdatePatient(PatientUpdateDto patientUpdateDto)
        {
            var valid = await _updateValidator.ValidateAsync(patientUpdateDto);
            
            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await _patientService.UpdatePatient(patientUpdateDto);
            if (result.Data == null)
            {
                return BadRequest(new
                {
                    statusCode = StatusCodes.Status404NotFound,
                    Message = result.Message
                });
            }
            return Ok(new
            {
                statusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result
            });


        }


    }
}


