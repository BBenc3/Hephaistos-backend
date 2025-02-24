using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectHephaistos.Data;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ProjectHephaistos.Models;
using ProjectHephaistos.Services;

namespace ProjectHephaistos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            ConfigurePipeline(app);

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey), "JWT SecretKey cannot be null or empty.");
            }

            builder.Services.AddSingleton<JwtHelper>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
         {
             options.SwaggerDoc("v1", new OpenApiInfo
             {
                 Title = "Project Hephaistos API",
                 Version = "v1",
                 Description = "API documentation for Project Hephaistos."
             });

             options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 Name = "Authorization",
                 Scheme = "Bearer",
                 BearerFormat = "JWT",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.Http,
                 Description = "Enter 'Bearer' followed by a space and your JWT token."
             });

             options.OperationFilter<AuthorizeCheckOperationFilter>();
         });


            builder.Services.AddDbContext<HephaistosContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 27)));
            });

            // Add ASP.NET Identity
            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<HephaistosContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(builder =>
                    builder.WithOrigins("http://localhost:3000", "https://another-example.com")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                )
            );

            // Add logging service
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<EmailService>(); // Ensure EmailService is registered before OtpService
            builder.Services.AddScoped<OtpService>();
            builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);

            builder.Services.Configure<FtpConfig>(builder.Configuration.GetSection("FtpConfig"));
            builder.Services.AddScoped<FtpService>();
        }

        private static void ConfigurePipeline(WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Add this line to serve static files

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Hephaistos API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors();

            // Use logging middleware
            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Handling request: {RequestPath}", context.Request.Path);
                await next.Invoke();
                logger.LogInformation("Finished handling request.");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
