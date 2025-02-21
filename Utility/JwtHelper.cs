﻿using Microsoft.IdentityModel.Tokens;
using ProjectHephaistos.Models; // Add this line to reference the RefreshToken model
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtHelper
{
    private readonly string _secretKey;
    private readonly int _tokenLifetimeInDays;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtHelper(IConfiguration configuration)
    {
        _secretKey = configuration["JwtSettings:SecretKey"];
        if (string.IsNullOrWhiteSpace(_secretKey))
        {
            throw new ArgumentNullException(nameof(_secretKey), "JWT secret key cannot be null or empty.");
        }

        var tokenLifetimeValue = configuration["JwtSettings:TokenLifetimeInDays"];
        if (string.IsNullOrWhiteSpace(tokenLifetimeValue) || !int.TryParse(tokenLifetimeValue, out _tokenLifetimeInDays))
        {
            throw new ArgumentException("JWT token lifetime must be a valid integer.", nameof(_tokenLifetimeInDays));
        }

        _issuer = configuration["JwtSettings:Issuer"];
        if (string.IsNullOrWhiteSpace(_issuer))
        {
            throw new ArgumentNullException(nameof(_issuer), "JWT issuer cannot be null or empty.");
        }

        _audience = configuration["JwtSettings:Audience"];
        if (string.IsNullOrWhiteSpace(_audience))
        {
            throw new ArgumentNullException(nameof(_audience), "JWT audience cannot be null or empty.");
        }
    }

    public string GenerateToken(int userId, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_tokenLifetimeInDays),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Extracts the user ID from the token.
    /// </summary>
    /// <param name="authorization"></param>
    /// <returns>userId(int) or null</returns>
    public int? ExtractUserIdFromToken(string authorization)
    {
        var token = authorization.Replace("Bearer ", "");

        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
        {
            return null;
        }

        var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        if (userIdClaim == null)
        {
            return null;
        }

        if (int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }

        return null;
    }
}
