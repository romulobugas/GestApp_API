using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GestApp_API.Configuration;
using GestApp_API.Models; // Certifique-se de que este namespace contém a classe UserLogin
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GestApp_API.Services
{
    public class AuthenticationService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly DbContext _context;

        public AuthenticationService(IOptions<JwtSettings> jwtSettings, DbContext context)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }

        public async Task<string> AuthenticateAsync(string username, string passwordHash)
        {
            // Validação do usuário
            var user = await _context.UserLogins
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(u => u.Login == username);

            if (user == null)
            {
                // Considerar lançar uma exceção específica para usuário não encontrado
                throw new UnauthorizedAccessException("Usuário não existe.");
            }

            if (user.Password != passwordHash) // Aqui, idealmente você compararia hashes de senha
            {
                throw new UnauthorizedAccessException("Usuário ou Senha incorretos.");
            }

            // Geração do token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            // Adicionando mais claims ao token para representar melhor o usuário
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, username),
                // Adicionar mais claims conforme necessário
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
