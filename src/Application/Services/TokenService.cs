using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CipherLock.Domain.Entities;
using CipherLock.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CipherLock.Application.Services;

public class TokenService(
    IConfiguration configuration
) : ITokenService
{
    public string Generate(User user)
    {
        var privateKey = configuration["Jwt:PrivateKey"]
            ?? throw new Exception();

        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(privateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = GenerateClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2),
        };

        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(
            new Claim(ClaimTypes.Name, user.Id.ToString())
        );
        ci.AddClaim(
            new Claim(ClaimTypes.Role, user.Role.ToString())
        );

        return ci;
    }
}
