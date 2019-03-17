using System;
using memoseeds.Database;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: 
        [HttpPost("/login")]
        public JsonResult login([FromBody]UserDataLogin data)
        {
            return Json("Hello, " + data.log + " " + data.pass);
        }

        // GET: 
        [HttpGet("/login")]
        public JsonResult login2()
        {
            return Json("Hello, ");
        }



        public class UserDataLogin
        {
            public string log { get; set; }
            public string pass { get; set; }
        }
    }
}