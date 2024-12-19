using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext dataContext, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExits(registerDto.Username)) return BadRequest("User name is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                Name = registerDto.Username.ToLower(),
                PassportHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password.ToLower())),
                PassportSalt = hmac.Key
            };

            await dataContext.AddAsync(user);
            await dataContext.SaveChangesAsync();

            return new UserDto
            {
                Username = user.Name,
                token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(
                x => x.Name == loginDto.Username.ToLower());

            if (user == null) return BadRequest(Unauthorized("Invalid user"));

            using var hmac = new HMACSHA512(user.PassportSalt);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PassportHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                Username = user.Name,
                token = tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExits(string userName)
        {
            return await dataContext.Users.AnyAsync(x => x.Name.ToLower() == userName.ToLower());
        }

    }
}
