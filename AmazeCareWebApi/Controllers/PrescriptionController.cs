using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.PrescriptionDtos;
using AmazeCareWebApi.Services.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCareWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPerscriptionService _perscriptionService;
        private readonly IValidator<PrescriptionCreateDto> _validator;
        private readonly IValidator<PrescriptionUpdateDto> _updateValidator;

        public PrescriptionController(IPerscriptionService service, IValidator<PrescriptionCreateDto> validator, IValidator<PrescriptionUpdateDto> updateValidator) { 
            _perscriptionService = service;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("/AddPrescription")]
        public async Task<IActionResult> AddPrescription(PrescriptionCreateDto prescriptionCreateDto)
        {
            var valid = await _validator.ValidateAsync(prescriptionCreateDto);

            if (!valid.IsValid)
            {

                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result=await _perscriptionService.AddPrescription(prescriptionCreateDto);
            if (!result.sucess)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.message
                });
            }
            return Ok(new
            {   StatusCode=StatusCodes.Status200OK,
                Message=result.message
            });
        }

        [Authorize(Roles = "Admin,Doctor, Patient")]
        [HttpGet("/GetPrescriptionBy{PrescriptionId}")]
        public async Task<IActionResult> GetPrescriptionById(int PrescriptionId)
        {
            var result = await _perscriptionService.GetPrescriptionById(PrescriptionId);
            if (!result.sucess)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.message
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.message,
                Data=result.data
            });
        }

        [Authorize(Roles = "Admin,Doctor, Patient")]
        [HttpGet("/AllPrescrpitionForPatient{RecordId}")]
        public async Task<IActionResult> GetAllPescrpitionForPatient(int RecordId) {
            var result = await _perscriptionService.GetPrescriptionsByRecordId(RecordId);
            if (!result.sucess)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.message
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("/UpdatePrescription")]
        public async Task<IActionResult> UpdatePescrption(PrescriptionUpdateDto prescriptionUpdateDto)
        {
            var valid = await _updateValidator.ValidateAsync(prescriptionUpdateDto);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }


            var result=await _perscriptionService.UpdatePrescription(prescriptionUpdateDto);
            if (!result.sucess)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.message
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.message,
                Data = result.data
            });
        }

    }
}
