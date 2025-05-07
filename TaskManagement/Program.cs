using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManagement.API.Swagger;
using TaskManagement.Core;
using TaskManagement.Core.Common.Authentication;
using TaskManagement.Core.Helpers;
using TaskManagement.Core.TypeFinder;
using TaskManagement.API.Middlewares.Authentication;
using TaskManagement.Core.Common.Configuration;
namespace TaskManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load JWT settings
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("Jwt"));
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JWTSettings>();

            // Register Dependency Injection
            ITypeFinder typeFinder = new TypeFinder();

            builder.Services.AddSingleton(typeFinder);


            // Register other services from all layers
            builder.Services.RegisterDependencies(typeFinder, builder.Configuration);

            // Register Mapster mapping profiles
            builder.Services.RegisterMappings();

            builder.Services.Configure<PaginationSettings>(builder.Configuration.GetSection("Pagination"));

            builder.Services.AddSingleton<JWTGenerator>();
            builder.Services.AddSingleton<Hashing>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();

            // Add JWT authentication

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                        };
                    });


            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagement API", Version = "v1" });

                // Add JWT Bearer definition
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter token Directly .. without 'Bearer' [space] \nExample: eyJhbGciOiJIUzI1..."
                });

                // Apply it globally
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                // Add custom refresh token header parameter
                c.OperationFilter<AddRefreshTokenHeader>();
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();

            app.UseMiddleware<AutoRefreshTokenMiddleware>();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
