using Application;
using Azure.Storage.Blobs;
using Core.Constants;
using Core.Enums;
using Infrastructure;
using Persistence;
using Persistence.Data;
using TemplateFormattedConfiguration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.EnableTemplatedConfiguration();

if (builder.Environment.IsDevelopment())
    builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("LocalSQLConnectionString")!);
else
{
    builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("AzureSQLConnectionString")!);
    var blobServiceClient = new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccountConnectionString"));
    var containerClient = blobServiceClient.GetBlobContainerClient(builder.Configuration.GetValue<string>("ImagesContainerName"));
    builder.Services.AddSingleton(provider => containerClient);
}

var configuration = builder.Configuration.GetSection("DefaultJwtConfig");
builder.Services.Configure<JwtConfiguration>(configuration);
builder.Services.AddAuthenticationConfigurations(configuration.Get<JwtConfiguration>()!);
builder.Services.AddMappers();
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Environment.IsDevelopment());

builder.Services.AddCors(options =>
{
    options.AddPolicy(builder.Configuration.GetSection("Policies:DefaultPolicy:Name").Value!,
        policyBuilder =>
        {
            var origin = builder.Configuration.GetSection("Policies:DefaultPolicy:Origin").Value!;
            policyBuilder.WithOrigins(builder.Configuration.GetSection("Policies:DefaultPolicy:Origin").Value!)
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
app.UseRouting();
app.UseCors(builder.Configuration.GetSection("Policies:DefaultPolicy:Name").Value!);
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await services.SeedData();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.AddHubs();
app.MapGet("/", () => "Hello World!");
app.Run();