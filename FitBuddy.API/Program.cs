using ECommerce.API.Helper.MiddleWares;
using FitBuddy.API.Helper.Extention;
using FitBuddy.ApplicationServer.AuthService;
using FitBuddy.Core.Entities;
using FitBuddy.Core.Services.Contract;
using FitBuddy.Infrastructure.DBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FitBuddy.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllersWithViews();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddDbContext<StoreDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("SQL"));
			});

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<StoreDbContext>().
				AddDefaultTokenProviders();
			builder.Services.AddApplicationServices();
			builder.Services.AddAuthServices(builder.Configuration);
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

			// Add CORS policy to allow all origins
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder =>
					{
						builder.AllowAnyOrigin()  // ?? Allow requests from any domain (Web, Mobile, etc.)
							   .AllowAnyHeader()  // ?? Allow all headers (Authorization, Content-Type, etc.)
							   .AllowAnyMethod(); // ?? Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
					});
			});
			var app = builder.Build();


			var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var DBcontext = services.GetRequiredService<StoreDbContext>();
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				var UserManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				await DBcontext.Database.MigrateAsync();
				await DbcontextSeed.SeedAsync(UserManager);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during apply the Migrations");
			}
			// Enable CORS globally
			app.UseCors("AllowAll");
			app.UseMiddleware<ExceptionHandlerMiddleware>();
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseStatusCodePagesWithReExecute("/Error/{0}");//best practice 
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();
			app.UseStaticFiles();

			app.MapControllers();

			app.Run();
		}
	}
}
