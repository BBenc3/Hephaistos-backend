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

            builder.Services.AddCors(options => 
                options.AddDefaultPolicy(builder => 
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                )
            );
        }

        private static void ConfigurePipeline(WebApplication app)
        {
            // Ensure HTTPS redirection is used.
            app.UseHttpsRedirection();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project Hephaistos API v1");
                    c.RoutePrefix = string.Empty; // Így a Swagger az alap URL-en érhető el
                });
            }
            app.UseCors();

            // Use authentication and authorization middleware.
            app.UseAuthentication(); // Authentication middleware comes first.
            app.UseAuthorization();  // Authorization middleware comes after.

            // Map controllers to endpoints
            app.MapControllers();
        }

    }
}
