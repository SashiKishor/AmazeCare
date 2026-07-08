
using AmazeCareWebApi.Data;
using AmazeCareWebApi.Dtos.User;
using AmazeCareWebApi.Mappings;
using AmazeCareWebApi.Middleware;
using AmazeCareWebApi.Models;
using AmazeCareWebApi.Repository.Implementation;
using AmazeCareWebApi.Repository.Interface;
using AmazeCareWebApi.Services.Implementation;
using AmazeCareWebApi.Services.Interface;
using AmazeCareWebApi.Validations.AppointmentValidations;
using AmazeCareWebApi.Validations.DoctorValidation;
using AmazeCareWebApi.Validations.MedicalRecordValidation;
using AmazeCareWebApi.Validations.patientValidation;
using AmazeCareWebApi.Validations.PerscriptionValidation;
using AmazeCareWebApi.Validations.UserValidation;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AmazeCareWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:5173",
                            "http://localhost:5174",
                            "http://localhost:3000"   
                          )
                          .AllowAnyHeader()  
                          .AllowAnyMethod();  
                });
            });


            builder.Services.AddControllers();
            builder.Services.AddDbContext<AppoinmentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            builder.Services.AddValidatorsFromAssemblyContaining<patientCreateDtoValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<PatientUpdateDtoValidation>();

            builder.Services.AddValidatorsFromAssemblyContaining<AppointmentCreateValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<AppointmentUpdateStatusDtoValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<AppointmentRescheduleDtoValidation>();

            builder.Services.AddValidatorsFromAssemblyContaining<DoctorCreateValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<DoctorUpdateValidation>();

            builder.Services.AddValidatorsFromAssemblyContaining<MedicalRecordCreateValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<MedicalRecordUpdateValidation>();

            builder.Services.AddValidatorsFromAssemblyContaining<PerscriptionCreateDtoValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<PerscriptionUpdateDtoValidation>();

            builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<UserCreateValidation>();
            builder.Services.AddValidatorsFromAssemblyContaining<AdminAccessCreateValidation>();

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPatientService, PatientService>();

            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();

            builder.Services.AddScoped<IAppointmentRepository, AppotimentRepository>();
            builder.Services.AddScoped<IAppotimentService, AppointmentService>();

            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IPerscriptionService, PescriptionService>();

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                string issuer = builder.Configuration["JwtSettings:Issuer"]!;
                string audience = builder.Configuration["JwtSettings:Audience"]!;
                string secretKey = builder.Configuration["JwtSettings:SecretKey"]!;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddLog4Net("log4net.config");

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token only. Do not type Bearer manually."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseCors("AllowReactApp");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        
                
        }
    }
}
