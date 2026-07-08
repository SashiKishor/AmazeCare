using AmazeCareWebApi.Dtos.AppointmentDtos;
using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Services.Interface;
using AmazeCareWebApi.Validations.AppointmentValidations;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCareWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppotimentService _appotimentService;
        private readonly IValidator<AppointmentCreateDto> _validator;
        private readonly IValidator<AppointmentUpdateStatusDto> _updateValidator;
        private readonly IValidator<AppointmentRescheduleDto> _rescheduleValidator;
        

        public AppointmentController(IAppotimentService appotimentService, IValidator<AppointmentCreateDto> validator, IValidator<AppointmentUpdateStatusDto> updateValidator, IValidator<AppointmentRescheduleDto> rescheduleValidator)
        {
            _appotimentService = appotimentService;
            _validator = validator;
            _updateValidator = updateValidator;
            _rescheduleValidator = rescheduleValidator;
        }


        [Authorize(Roles = "Admin, Patient")]
        [HttpPost("/CreateNewAppointment")]
        public async Task<IActionResult> AddAppointment(AppointmentCreateDto appointmentCreateDto) {
            var valid = await _validator.ValidateAsync(appointmentCreateDto);

            if (!valid.IsValid)
            {

                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result=await _appotimentService.AddAppointment(appointmentCreateDto);
            if (!result.Success)
            {
                return BadRequest(new
                {
                    StatusCode=StatusCodes.Status400BadRequest,
                    Message=result.Message,
                    Data=result.data
                });
            }

            return Ok(new
            {
                StatusCode=StatusCodes.Status200OK,
                Message=result.Message,
                Data= result.data
            });

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/GetAllAppointments")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var result=await _appotimentService.GetAllAppointments();
            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "All Appointments Retrived.",
                Data = result
            });
        }

        [Authorize(Roles = "Admin, Patient")]
        [HttpGet("/AppointmentById{AppointmentId}")]
        public async Task<IActionResult> GetAppointmentById(int AppointmentId) { 
            var result=await _appotimentService.GetAppointmentById(AppointmentId);
            if(!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpGet("/UpcomingAppointmentsOfDoctors{doctorId}")]
        public async Task<IActionResult> UpcomingAppointmentsOfDoctor(int doctorId)
        {
            var result=await _appotimentService.GetUpcomingAppointmentForDoctor(doctorId);
            if (!result.sucess)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Admin, Doctor")]
        [HttpGet("/AllAppointmentsOfTheDoctor{doctorId}")]
        public async Task<IActionResult> AllAppointmentsOfTheDoctor(int doctorId)
        {
            var result = await _appotimentService.GetAppointmentByDoctor(doctorId);
            if (!result.sucess)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("/UpcomingAppointmentsOfPatient{patientId}")]
        public async Task<IActionResult> UpcomingAppointmentsOfPatient(int patientId)
        {
            var result = await _appotimentService.GetUpcomingAppointmentForPatient(patientId);
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("/AllAppointmentsOfThePatient{patientId}")]
        public async Task<IActionResult> AllAppointmentsOfThePatient(int patientId)
        {
            var result = await _appotimentService.GetAppointmentByPatient(patientId);
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/UpcomingAppointments")]
        public async Task<IActionResult> AllUpcomingAppointment()
        {
            var result=await _appotimentService.GetAllUpcomingAppointments();
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/RequestedAppointments")]
        public async Task<IActionResult> AllRequestedAppointment()
        {
            var result = await _appotimentService.GetAllRequestedAppointments();
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Admin, Doctor,Patient")]
        [HttpPut("/UpdateStatus")]
        public async Task<IActionResult> UpdateAppointmentStatus(AppointmentUpdateStatusDto statusDto)
        {
            var valid = await _updateValidator.ValidateAsync(statusDto);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result=await _appotimentService.UpdateAppointmentStatus(statusDto);
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.data
            });
        }

        [Authorize]
        [HttpPut("/ResheduleAppointment")]
        public async Task<IActionResult> ResheduleAppointment(AppointmentRescheduleDto rescheduleDto)
        {
            var valid = await _rescheduleValidator.ValidateAsync(rescheduleDto);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await _appotimentService.RescheduleAppointment(rescheduleDto);
            if (!result.Success)
            {
                return NotFound(new
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = result.Message,
                    Data = result.data
                });
            }

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Message = result.Message,
                Data = result.data
            });
        }

        [Authorize(Roles = "Admin,Patient")]
        [HttpDelete("/DeleteAppointment{AppointmentId}")]
        public async Task<IActionResult> DeleteAppointment(int AppointmentId)
        {
            var result = await _appotimentService.RemoveAppointment(AppointmentId);
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


    }
}
