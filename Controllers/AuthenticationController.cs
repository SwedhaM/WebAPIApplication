using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace Signup.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _db;

        public AuthenticationController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _db = context;
        }

        [HttpPost]
        public async Task<IActionResult> signUp(SignUpData signUpData)
        {
            try
            {
                bool emailExists = await _db.signUpDatas.AnyAsync(signUp => signUp.Email == signUpData.Email);
                if (emailExists)
                {
                    return BadRequest("Email already registered");
                }
                if (signUpData.Password != signUpData.ConfirmPassword)
                {
                    return BadRequest("Password and Confirm Password must match");
                }
                _db.signUpDatas.Add(signUpData);
                await _db.SaveChangesAsync();

                return Ok(signUpData);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Internal server error: {exception.Message}");
            }
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> login(LoginData loginData)
        {
            try
            {
                var user = await _db.signUpDatas.SingleOrDefaultAsync(s => s.Email == loginData.Email && s.Password == loginData.Password && s.Role == loginData.Role);
                if (user == null)
                {
                    return Unauthorized("Invalid email or password");
                }

                var token = generateJwtToken(user);

                return Ok(new { token,user.Id,user.Name,user.Email,user.PhoneNumber,user.Address });
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Internal server error: {exception.Message}");
            }
        }

        private string generateJwtToken(SignUpData user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] //the user's identity information that will be included in the token.
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],//provider
                audience: _configuration["Jwt:Audience"],//acceptor
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
