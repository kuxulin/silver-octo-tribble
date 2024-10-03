using Persistence;
using Infrastructure;
using Core.Constants;
using Application;
using Persistence.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthenticationConfigurations(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>());
builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("ServerConnection"));
builder.Services.AddMappers();
builder.Services.AddRepositories();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy(builder.Configuration.GetSection("Policies:LocalPolicy:Name").Value,
        policyBuilder =>
        {
            policyBuilder.WithOrigins(builder.Configuration.GetSection("Policies:LocalPolicy:Origin").Value)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await services.SeedAdminAndRoles();
    await services.SeedUsers();
}

app.UseAuthorization();
app.UseCors(builder.Configuration.GetSection("Policies:LocalPolicy:Name").Value);
app.MapControllers();

app.Run();