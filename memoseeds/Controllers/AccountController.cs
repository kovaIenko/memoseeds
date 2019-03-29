using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using memoseeds.Database;
using memoseeds.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace memoseeds.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private ApplicationDbContext db;
        private IConfiguration _config;

        public AccountController(ApplicationDbContext context, IConfiguration config)
        {
             db = context;
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public void log()
        {
            db.Users.Add(new User()
            {
                Username = "kovalenko",
                Email = "ruskov@gmail.com",
                Password="qwerty123",
                Credits = 5000


            });
            db.SaveChanges();
        }

        // POST:
        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody]UserAuthenticateData login)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUserAsync(login);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
       // new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
       // new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User> AuthenticateUserAsync(UserAuthenticateData data)
        {
            User user = await db.Users.FirstOrDefaultAsync(
            u => u.Username == data.Username &&
                 u.Password == data.Password
            );
            return user;
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> Register(UserRegisterData data)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => 
                    u.Username == data.Username ||
                    u.Email == data.Email
                    );
                if (user == null)
                {
                    // adding user to db
                    user = new User
                    {
                        Username = data.Username,
                        Email = data.Email,
                        Password = data.Password
                    };
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return Ok(new { token = GenerateJSONWebToken(user) });
                }
                else
                    ModelState.AddModelError("ErrorStack", "This login or email is already taken");
            }
            return BadRequest(ModelState);
        }

        public class UserAuthenticateData
        {
            [Required(ErrorMessage = "Username not specified")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password not specified")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class UserRegisterData
        {
            [Required(ErrorMessage = "Username not specified")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Email not specified")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password not specified")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}