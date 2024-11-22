using Core.Constants;
using Core.Interfaces.Services;
using Infrastructure.Mappings;
using Infrastructure.Services;
using Infrastructure.Services.ImageContent;
using Infrastructure.SignalR.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProjectProfile).Assembly);
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, bool isDevelopment)
    {
        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<INotificationsHubService, NotificationsHubService>();
        services.AddSingleton<IImageTransformingService, ImageTransformingService>();

        if (isDevelopment)
            services.AddScoped<IImageContentService, ImageContentLocalService>();
        else
            services.AddScoped<IImageContentService, ImageContentBlobStorageService>();

        return services;
    }

    public static void AddAuthenticationConfigurations(this IServiceCollection services, JwtConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters() //TODO move validation parameters in some constants place (i use this config in dependency injection while registering jwt auth too) ask chatgpt
                {
                    ValidIssuer = configuration.Issuer,
                    ValidAudience = configuration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key)), 
                    NameClaimType = DefinedClaim.Name,
                    RoleClaimType = DefinedClaim.Role,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/onlineStatusHub") || path.StartsWithSegments("/notificationsHub")))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };

            });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Todo app Api",
            });

            var securityDefinition = new OpenApiSecurityScheme
            {
                Name = "Bearer",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Specify the authorization token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
            };

            swagger.AddSecurityDefinition("JWT_AUTH", securityDefinition);

            var securityRequirements = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "JWT_AUTH" } },
                    new List<string>(0)
                }
            };

            swagger.AddSecurityRequirement(securityRequirements);
        });
    }

    public static void AddHubs(this WebApplication app)
    {
        app.MapHub<OnlineStatusHub>("/onlineStatusHub");
        app.MapHub<NotificationsHub>("/notificationsHub");
    }
}
