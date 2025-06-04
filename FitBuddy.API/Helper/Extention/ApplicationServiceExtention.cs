using ECommerce.API.Helper.Error;
using FitBuddy.ApplicationServer.AuthService;
using FitBuddy.ApplicationServer.EmilService;
using FitBuddy.Core.Repositroy.Contract;
using FitBuddy.Core.Services.Contract;
using FitBuddy.Infrastructure.Repositroies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FitBuddy.API.Helper.Extention
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actioncontext)
                =>
                {
                    //Select Errors in the ModelState for each Key that have error
                    var Errors = actioncontext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                        .SelectMany(P => P.Value.Errors)
                                                        .Select(E => E.ErrorMessage)
                                                        .ToArray();
                    //Beacuse its a group Then Select form each group the message
                    var Response = new ApiValidationResponse()
                    {
                        Errors = Errors
                    };
                    return new BadRequestObjectResult(Response);

                };

            });
            services.AddScoped(typeof(IGenericRepositroy<>),typeof(GenericRepositroy<>));
            services.AddScoped(typeof(IExercisesRepository),typeof(ExercisesRepsoitroy));
            services.AddScoped(typeof(ICloudinaryService),typeof(CloudinaryService));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IEmailService), typeof(EmailService));
            return services;
        }
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"] ?? string.Empty)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero

                    };
                })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                options.SaveTokens=true;
                });
			return services;
        }
    }
}
