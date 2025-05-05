using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManagement.API.Swagger
{
    public class AddRefreshTokenHeader : IOperationFilter
    {
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
