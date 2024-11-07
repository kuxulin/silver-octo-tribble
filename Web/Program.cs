using Persistence;
using Infrastructure;
using Core.Constants;
using Application;
using Persistence.Data;
using Core.Enums;
using Infrastructure.SignalR.Hubs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthenticationConfigurations(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>()!);

if(builder.Environment.IsDevelopment())
    builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("LocalServerConnectionString")!);
else
    builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("AzureServerConnectionString")!);

builder.Services.AddMappers();
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy(builder.Configuration.GetSection("Policies:LocalPolicy:Name").Value!,
        policyBuilder =>
        {
            policyBuilder.WithOrigins(builder.Configuration.GetSection("Policies:LocalPolicy:Origin").Value!)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(DefinedPolicy.AdminPolicy, policy => policy.RequireRole(AvailableUserRole.Admin.ToString()))
    .AddPolicy(DefinedPolicy.DefaultPolicy, policy => policy.RequireClaim(DefinedClaim.IsBlocked,false.ToString()));

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder.Configuration.GetSection("Policies:LocalPolicy:Name").Value!);
app.UseAuthentication();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.SeedData();
}

app.UseAuthorization();
app.MapControllers();
app.AddHubs();

app.Run();