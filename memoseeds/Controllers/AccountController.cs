using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using memoseeds.Database;
using memoseeds.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        // GET: 
        [HttpGet("/login")]
        public JsonResult Login()
        {
            return Json("hello, blyad");
        }

        // POST:
        [AllowAnonymous]
        [HttpPost("/login")]
        public IActionResult Login([FromBody]UserLoginData login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }

        private string GenerateJSONWebToken(UserLoginData userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.Login),
       // new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
       //new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        

        private UserLoginData AuthenticateUser(UserLoginData data)
        {
            //User user = await db.Users.FirstOrDefaultAsync(
                    //u => u.Name == data.Login &&
                    //     u.Password == data.Password
                    //);
            // if (data != null) return data;
            return data;

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(UserRegisterData data)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = await db.Users.FirstOrDefaultAsync(u => u.Name == data.Login);
        //        if (user == null)
        //        {
        //            // adding user to db
        //            db.Users.Add(new User { Name = data.Login, Email = data.Email, Password = data.Password });
        //            await db.SaveChangesAsync();

        //            await Authenticate(data.Login); // authentification
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //            ModelState.AddModelError("", "Incorrect login and(or) password");
        //    }
        //    return View(data);
        //}

        //private async Task Authenticate(string userName)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
        //    };
        //    ClaimsIdentity id = new ClaimsIdentity(
        //        claims,
        //        "ApplicationCookie",
        //        ClaimsIdentity.DefaultNameClaimType,
        //        ClaimsIdentity.DefaultRoleClaimType
        //        );
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        //}

        public class UserLoginData
        {
            [Required(ErrorMessage = "Login not specified")]
            public string Login { get; set; }

            [Required(ErrorMessage = "Password not specified")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class UserRegisterData
        {
            [Required(ErrorMessage = "Login not specified")]
            public string Login { get; set; }

            [Required(ErrorMessage = "Email not specified")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password not specified")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}