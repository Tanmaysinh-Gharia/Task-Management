using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManagement.API.Swagger
{
    public class AddRefreshTokenHeader : IOperationFilter
    {
        /// <summary>
        /// Adds a custom optional header parameter "X-Refresh-Token" to Swagger documentation
        /// if it's not already present. This allows users to manually provide a refresh token
        /// in the request headers during testing via Swagger UI.
        /// </summary>

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            if (!operation.Parameters.Any(p => p.Name == "X-Refresh-Token"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-Refresh-Token",
                    In = ParameterLocation.Header,
                    Required = false,
                    AllowEmptyValue = true,
                    Schema = new OpenApiSchema { Type = "string" },
                    Description = "Enter your refresh token manually"
                });
            }
        }
    }
}
