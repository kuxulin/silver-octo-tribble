﻿using Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Interfaces.Services;
public interface ITokenService
{
    Task<string> CreateAccessTokenAsync(User user, IEnumerable<string> userRoles);
    string CreateRefreshToken(string username);
    string GetNameFromToken(string token);
    bool ValidateToken(string token);
}
