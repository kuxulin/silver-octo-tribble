using Core.Constants;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;
internal class TokenService : ITokenService
{
    private readonly JwtConfiguration _configuration;

    public TokenService(UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    public async Task<string> CreateAccessTokenAsync(User user,IEnumerable<string> userRoles)
    {
        var authClaims = new List<Claim>
        {
            new Claim("Id",user.Id.ToString()),
            new Claim("Name", user.UserName),
            new Claim("Email", user.Email),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim("Role", userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.UtcNow.AddMinutes(5),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken(string username)
    {
        var authClaims = new List<Claim>
        {
            new Claim("Name", username),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.UtcNow.AddDays(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal GetTokenPrincipal(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var validation = new TokenValidationParameters //TODO move validation parameters in some constants place (i use this config in dependency injection while registering jwt auth too)
        {
            IssuerSigningKey = securityKey,
            ValidIssuer = _configuration.Issuer,
            ValidAudience = _configuration.Audience,
            NameClaimType = "Name",
            RoleClaimType = "Role"
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }
}
