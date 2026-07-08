using AmazeCareWebApi.Dtos.DoctorDtos;
using AmazeCareWebApi.Dtos.User;
using AmazeCareWebApi.Services.Interface;
using Azure;
using Azure.Core;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCareWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginRequestDto> _validator;
        private readonly IValidator<UserCreateDto> _userValidator;
        private readonly IValidator<AdminAccessCreateDto> _adminValidator;


        public AuthController(IAuthService authService, IValidator<LoginRequestDto> validator, IValidator<UserCreateDto> userValidator, IValidator<AdminAccessCreateDto> adminValidator)
        {
            _authService = authService;
            _validator = validator;
            _userValidator = userValidator;
            _adminValidator = adminValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var valid = await _validator.ValidateAsync(request);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }
            var response = await _authService.LoginAsync(request);

            if (!response.Success)
            {
                return Unauthorized(new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = response.Message
                });
            }

            return Ok(new
            {
                StatusCode=StatusCodes.Status200OK,
                Message=response.Message,
                Data=response.Data
            });
        }

        [HttpPost("/CreateNewUser")]
        public async Task<IActionResult> createUser(UserCreateDto user)
        {
            var valid = await _userValidator.ValidateAsync(user);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result =await _authService.CreateUserAsync(user);
            if (!result.Sucess)
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

        [Authorize(Roles ="Admin")]
        [HttpPost("/AdminAccessCreate")]
        public async Task<IActionResult> createDoctorAdminUser(AdminAccessCreateDto user)
        {
            var valid = await _adminValidator.ValidateAsync(user);

            if (!valid.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = valid.Errors.Select(e => e.ErrorMessage)
                });
            }

            var result = await _authService.CreateUsers(user);
            if (!result.Sucess)
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
                Message = result.Message,
                UserId = result.UserId
            });
        }


    }
}
