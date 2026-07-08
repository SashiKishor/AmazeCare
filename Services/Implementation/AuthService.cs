using AmazeCareWebApi.Dtos.User;
using AmazeCareWebApi.Exceptions.UserException;
using AmazeCareWebApi.Migrations;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Interface;

namespace AmazeCareWebApi.Services.Implementation
{
    public class AuthService:IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthService(IAuthRepository authRepository, IJwtTokenService jwtTokenService)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<(bool Sucess,string Message)> CreateUserAsync(UserCreateDto user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var data=new User {
                UserName=user.UserName,
                FullName=user.FullName,
                Email=user.Email,
                PasswordHash= hashedPassword,
                Patient = new Patient
                {
                    FullName = user.FullName,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    ContactNumber = user.ContactNumber
                }
            };
            await _authRepository.CreateUserAsync(data);
            return (true, "Sucessfully created.");
        }

        public async Task<(bool Sucess, string Message, int? UserId)> CreateUsers(AdminAccessCreateDto user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var data = new User
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PasswordHash = hashedPassword,
                Role = user.Role
            };

            if (user.Role == "Doctor")
            {
                data.Doctor = new Doctor
                {
                    DoctorName = user.FullName,
                    Speciality = user.Speciality ?? "General Medicine",
                    Experience = user.Experience ?? 0f,
                    Qualification = user.Qualification ?? "MBBS",
                    Designation = user.Designation ?? "Resident"
                };
            }

            await _authRepository.CreateUserAsync(data);
            return (true, "Sucessfully created.", data.UserId);
        }

        public async Task<(bool Success, string Message, LoginResponceDto? Data)> LoginAsync(LoginRequestDto loginRequestDto)
        {
            try
            {
                var user = await _authRepository.GetUserByUserNameAsync(loginRequestDto.UserName);
                if (user == null)
                {
                    throw new UserNotFoundException("Username not found.");
                }
                if (!user.IsActive)
                {
                    throw new UserNotActiveException("User account is inactive");
                }
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    throw new InvalidPasswordException("Invalid username and password");
                }

                int? profileId = null;
                if (user.Role == "Patient")
                {
                    profileId = user.Patient?.PatientId;
                }
                else if (user.Role == "Doctor")
                {
                    profileId = user.Doctor?.DoctorId;
                }

                string token = _jwtTokenService.GenerateToken(user, out DateTime expiresAt);
                var response = new LoginResponceDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Role = user.Role,
                    Token = token,
                    TokenExpiresAt = expiresAt,
                    ProfileId = profileId
                };
                return (true, "Login Succesful", response);
            }
            catch (UserNotFoundException ex)
            {
                return (false, ex.Message, null);
            }
            catch (UserNotActiveException ex)
            {
                return (false, ex.Message, null);
            }
            catch (InvalidPasswordException ex)
            {
                return (false, ex.Message, null);
            }

        }

    }
}
