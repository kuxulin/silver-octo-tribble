using Persistence;
using Infrastructure;
using Core.Constants;
using Application;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthenticationConfigurations(builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>());
builder.Services.AddDbAndIdentity(builder.Configuration.GetConnectionString("ServerConnection"));
builder.Services.AddMappers();
builder.Services.AddRepositories();
builder.Services.AddServices();
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
app.UseAuthorization();

app.MapControllers();

app.Run();