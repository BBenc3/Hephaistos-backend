using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Ellenőrizzük, hogy az adott végpont vagy a kontroller tartalmaz-e [Authorize] attribútumot
        var hasAuthorize = context.MethodInfo
            .DeclaringType.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any()
            || context.MethodInfo.GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any();

        if (hasAuthorize)
        {
            // Hozzáadjuk a JWT Bearer hitelesítési követelményt a végponthoz
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
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
                }
            };
        }
    }
}
