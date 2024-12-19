using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string CreateToken(AppUser user)
        {
            var tokenKey = configuration["TokenKey"]
                ?? throw new Exception("can not access the tokenKey from appsetting");
            if (tokenKey.Length < 64) throw new Exception("Your token key need to be longer");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claim = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier,user.Name)
            };

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(claim),
                Expires=DateTime.UtcNow.AddDays(8),
                SigningCredentials=creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
