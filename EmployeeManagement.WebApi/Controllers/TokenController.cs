using EmployeeManagement.Infra.Context;
using EmploymentManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly EmployeeContext _context;

        public TokenController(IConfiguration config, EmployeeContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ApplicationUser _user)
        {
            if (_user != null && _user.Email != null && _user.Password != null)
            {
                var appUser = await GetUser(_user.Email, _user.Password);

                if (appUser != null)
                {

                    var claims = new[] {
                          new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                          new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                          new Claim("UserId", appUser.UserId.ToString()),
                          new Claim("DisplayName", appUser.DisplayName),
                          new Claim("UserName", appUser.UserName),
                          new Claim("Email", appUser.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                        );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                }
                else { return BadRequest("Invalid credentials"); }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<ApplicationUser?> GetUser(string email, string password)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
