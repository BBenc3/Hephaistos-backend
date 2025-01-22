using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectHephaistos.Data;
using System.Text;

namespace ProjectHephaistos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder);

            // Build the app.
            var app = builder.Build();

            // Configure the middleware pipeline.
            ConfigurePipeline(app);

            // Run the app.
            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add custom services.
            builder.Services.AddTransient<EmailService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.Configure<FtpConfig>(builder.Configuration.GetSection("FtpConfig"));

            // Register JwtHelper
            builder.Services.AddSingleton<JwtHelper>();

            // Add controllers and endpoints.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Configure Swagger with JWT authentication.
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
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your JWT token.\n\nExample: 'Bearer abc123token'"
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
                        Array.Empty<string>() // No specific scopes required
                    }
                });
            });

            // Configure database context with MySQL.
            builder.Services.AddDbContext<HephaistosContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 27)));
            });

            // Configure authentication and JWT.
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

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
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add CORS policy.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()  // You can specify specific origins here like .WithOrigins("https://example.com")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }

        private static void ConfigurePipeline(WebApplication app)
        {
            // Ensure HTTPS redirection is used.
            app.UseHttpsRedirection();

            // Apply CORS policy
            app.UseCors("AllowAll");  // Applying the "AllowAll" CORS policy

            // Use authentication and authorization middleware.
            app.UseAuthentication(); // Authentication middleware comes first.
            app.UseAuthorization();  // Authorization middleware comes after.

            // Map controllers to endpoints
            app.MapControllers();

            // Apply any pending migrations automatically.
            ApplyMigrations(app);
        }

        private static void ApplyMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HephaistosContext>();
            dbContext.Database.Migrate();
        }
    }
}
