﻿using Core.Constants;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;
internal class TokenService : ITokenService
{
    private readonly JwtConfiguration _configuration;

    public TokenService(IOptions<JwtConfiguration> configuration)
    {
        _configuration = configuration.Value;
    }

    public string CreateAccessToken(User user, IEnumerable<string> userRoles)
    {
        var authClaims = new List<Claim>
        {
            new Claim(DefinedClaim.Id,user.Id.ToString()),
            new Claim(DefinedClaim.Name, user.UserName!),
            new Claim(DefinedClaim.IsBlocked, user.IsBlocked.ToString())
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(DefinedClaim.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.UtcNow.AddMinutes(10),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken(string username)
    {
        var authClaims = new List<Claim>
        {
            new Claim(DefinedClaim.Name, username),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var token = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Issuer,
            expires: DateTime.UtcNow.AddDays(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key));

        var validation = new TokenValidationParameters //TODO move validation parameters in some constants place (i use this config in dependency injection while registering jwt auth too)
        {
            IssuerSigningKey = securityKey,
            ValidIssuer = _configuration.Issuer,
            ValidAudience = _configuration.Issuer
        };

        try
        {
            new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public string GetFieldFromToken(string token, string fieldName)
    {
        var parsedToken = new JwtSecurityToken(token);
        return parsedToken.Claims.First(c => c.Type == fieldName).Value;
    }
}
