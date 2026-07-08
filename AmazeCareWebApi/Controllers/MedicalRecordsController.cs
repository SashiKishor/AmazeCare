using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.MedicalRecordDtos;
using AmazeCareWebApi.Services.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCareWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IValidator<MedicalRecordCreateDto> _validator;
        private readonly IValidator<MedicalRecordUpdateDto> _updateValidator;
        public MedicalRecordsController(IMedicalRecordService recordService, IValidator<MedicalRecordCreateDto> validator, IValidator<MedicalRecordUpdateDto> updateValidator)
        {
            _medicalRecordService = recordService;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpPost("/AddMedicalRecord")]
        public async Task<IActionResult> addMedicalRecord(MedicalRecordCreateDto dto)
        {
            var valid = await _validator.ValidateAsync(dto);

            if (!valid.IsValid)
            {

                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result =await _medicalRecordService.AddMedicalRecordAsync(dto);
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status201Created,
                Message = result.Message
            });
        }
        
        [Authorize]
        [HttpGet("/GetReportfor{RecordId}")]
        public async Task<IActionResult> getRecordById(int RecordId)
        {
            var result=await _medicalRecordService.GetMedicalRecordByIdAsync(RecordId);
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Data
            });
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("/UpdateMedicalRecord")]
        public async Task<IActionResult> updateMedicalRecord(MedicalRecordUpdateDto record) {
            var valid = await _updateValidator.ValidateAsync(record);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await _medicalRecordService.UpdateMedicalRecord(record);
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message
                });
            }
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.Data
            });
        }
    }
}
