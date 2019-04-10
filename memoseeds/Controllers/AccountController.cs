using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using memoseeds.Models;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace memoseeds.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private IUserRepository UserRepository;
        private IConfiguration _config;
        private HashPassword HashClass;

        public AccountController(IUserRepository UserRepositor, IConfiguration config, HashPassword HashClass)
        {
            this.UserRepository = UserRepositor;
            this._config = config;
            this.HashClass = HashClass ?? throw new ArgumentNullException(nameof(HashClass));
        }

        // POST:
        [AllowAnonymous]
        [HttpPost("/login")]
        public IActionResult Login([FromBody]UserAuthenticateData login)
        {
            IActionResult response = Unauthorized();
            User user;
            if (login.IsUsername)
                user = UserRepository.GetUserByName(login.Username);
            else
                user = UserRepository.GetUserByEmail(login.Username);

            if (user != null)
            {
                string hashedPass = HashPassword.Encrypt(login.Password);
                if (user.Password.Equals(hashedPass))
                {
                    var tokenString = GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString, info = user });
                }
                else
                    response = Ok(new { Error = "Incorrect password." });
            }
            else
                response = Ok(new { Error = "User with that username not found" });
            return response;
        }


        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
       //     new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
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

        [HttpPost("/signup")]
        public IActionResult Register([FromBody]UserRegisterData data)
        {
            IActionResult response = Unauthorized();
            User user = UserRepository.GetUserByEmail(data.Email);
            if (user != null)
                response = Ok(new { Error = "This email is already taken." });
            user = UserRepository.GetUserByName(data.Username);
            if (user != null)
                response = Ok(new { Error = "This username is already taken." });
            if (user == null)
            {
                // adding user to db
                user = new User
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = HashPassword.Encrypt(data.Password)
                };
                UserRepository.Insert(user);
                response = Ok(new { token = GenerateJSONWebToken(user), info = user });
            }
            return response;
        }

        [HttpPost("/fbsignup")]
        public IActionResult Fbsignup([FromBody]UserRegisterData data)
        {
            User user = UserRepository.GetUserByEmail(data.Email);
            if (user != null)
                return Login(new UserAuthenticateData
                {
                    Username = data.Email,
                    IsUsername = false,
                    Password = data.Password
                });
            else
                return Register(data);
        }

        public class UserAuthenticateData
        {
            [Required(ErrorMessage = "Username not specified")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Bool not specified")]
            public bool IsUsername { get; set; }

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

    public class UserData
    {
        [Required(ErrorMessage = "Id not specified")]
        public long id { get; set; }

        [Required(ErrorMessage = "Image not specified")]
        public Byte[] image { get; set; }
    }
}