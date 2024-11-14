using Persistence;
using Infrastructure;
using Core.Constants;
using Application;
using Persistence.Data;
using Core.Enums;
using TemplateFormattedConfiguration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.EnableTemplatedConfiguration();
IConfiguration configuration;

if (builder.Environment.IsDevelopment())
{
    configuration = builder.Configuration.GetSection("LocalJwtConfig");
    builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("LocalSQLConnectionString")!);
}
else
{
    configuration = builder.Configuration.GetSection("ProductionJwtConfig");
    builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("AzureSQLConnectionString")!);
}

builder.Services.Configure<JwtConfiguration>(configuration);
builder.Services.AddAuthenticationConfigurations(configuration.Get<JwtConfiguration>()!);
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
    options.AddPolicy(builder.Configuration.GetSection("Policies:ProductionPolicy:Name").Value!,
       policyBuilder =>
       {
           policyBuilder.WithOrigins(builder.Configuration.GetSection("Policies:ProductionPolicy:Origin").Value!)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
       });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(DefinedPolicy.AdminPolicy, policy => policy.RequireRole(AvailableUserRole.Admin.ToString()))
    .AddPolicy(DefinedPolicy.DefaultPolicy, policy => policy.RequireClaim(DefinedClaim.IsBlocked, false.ToString()));

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    app.UseCors(builder.Configuration.GetSection("Policies:LocalPolicy:Name").Value!);
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await services.SeedData();
}
else
    app.UseCors(builder.Configuration.GetSection("Policies:ProductionPolicy:Name").Value!);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.AddHubs();
app.MapGet("/", () => "Hello World!");
app.Run();