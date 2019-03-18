using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using memoseeds.Database;
using memoseeds.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace memoseeds.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext db;

        public AccountController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: 
        [HttpGet("/login")]
        public IActionResult Login()
        {
            return View();
        }
        // POST: 
        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]UserLoginData data)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(
                    u => u.Name == data.Login &&
                         u.Password == data.Password
                    );
                if(user != null)
                {
                    await Authenticate(data.Login);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(data);
        }




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterData data)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Name == data.Login);
                if (user == null)
                {
                    // adding user to db
                    db.Users.Add(new User { Name = data.Login, Email = data.Email, Password = data.Password });
                    await db.SaveChangesAsync();

                    await Authenticate(data.Login); // authentification

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(data);
        }





        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
                );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }




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