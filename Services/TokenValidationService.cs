using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GestApp_API.Services
{

    public class TokenValidationService
    {
        private readonly IConfiguration _configuration;

        public TokenValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenValidationResult ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Verifica se o token é um JWT com uma chave de assinatura válida
                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var expDate = jwtToken.ValidTo;
                    var timeRemaining = expDate - DateTime.UtcNow;

                    return new TokenValidationResult
                    {
                        IsValid = true,
                        Username = principal.Identity.Name,
                        TimeRemaining = timeRemaining
                    };
                }
            }
            catch (Exception ex)
            {
                // Em caso de falha na validação, retorne um resultado inválido com a mensagem de erro
                return new TokenValidationResult { IsValid = false, ValidationError = ex.Message };
            }

            return new TokenValidationResult { IsValid = false, ValidationError = "Invalid token." };
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Ajuste conforme necessário
            };
        }
    }

    public class TokenValidationResult
    {
        public bool IsValid { get; set; }
        public string Username { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public string ValidationError { get; set; }
    }
}