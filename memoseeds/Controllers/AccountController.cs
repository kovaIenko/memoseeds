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
        [HttpGet("/hello")]
        public JsonResult Get()
        {
            return Json("Hello, we are memoseeds");
        }
    }
}