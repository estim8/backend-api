using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Estim8.Backend.Api.Security
{
    public interface ISecurityTokenService
    {
        string IssueToken(Guid gameId, Guid playerId, string[] roles);
    }

    public class SecurityTokenService : ISecurityTokenService
    {
        private readonly IConfiguration _configuration;

        public SecurityTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string IssueToken(Guid gameId, Guid playerId, string[] roles)
        {
            var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"]);
            var expiryDuration = int.Parse(_configuration["Jwt:ExpiryDuration"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,              // Not required as no third-party is involved
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("game_id", gameId.ToString()),
                    new Claim("player_id", playerId.ToString()),
                    new Claim(ClaimTypes.Role, string.Join(',', roles))
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(jwtToken);
        }
    }
}