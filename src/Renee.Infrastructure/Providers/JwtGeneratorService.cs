using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Renee.Infrastructure.Providers;

public class JwtGeneratorService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtGeneratorService(IConfiguration configuration)
    {
        _secretKey = configuration.GetValue<string>("JwtGenerationKey:SecretKey")
            ?? throw new InvalidOperationException("JwtGenerationKey:SecretKey is missing.");
        _issuer = configuration.GetValue<string>("JwtGenerationKey:issuer")
            ?? throw new InvalidOperationException("JwtGenerationKey:issuer is missing.");
        _audience = configuration.GetValue<string>("JwtGenerationKey:audience")
            ?? throw new InvalidOperationException("JwtGenerationKey:audience is missing.");
    }

    public string CreateJwtHs256(
        IDictionary<string, string>? userClaimsDictionary = null,
        TimeSpan? ttl = null)
    {
        var keyBytes = Convert.FromBase64String(_secretKey);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;

        var claims = userClaimsDictionary != null
            ? userClaimsDictionary.Select(kvp => new Claim(kvp.Key, kvp.Value)).ToList()
            : new List<Claim>();
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: now,
            expires: now.Add(ttl ?? TimeSpan.FromMinutes(60)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
