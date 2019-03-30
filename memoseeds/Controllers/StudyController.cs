using System;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{

    [ApiController]
    public class StudyController:  Controller
    {
        private IModuleRepository moduleRepository;
        private IUserRepository userRepository;

            public StudyController(IModuleRepository ModuleRepository, IUserRepository userRepository)
        {
            this.moduleRepository = ModuleRepository;
            this.userRepository = userRepository;
        }

        [HttpGet("api/getUsers")]
        public JsonResult getUs()
        {
            return Json(userRepository.getAllUsers());
        }

        [HttpPost("api/getModule")]
        public JsonResult getModule([FromBody]long id)
        {
            Module m = moduleRepository.GetById(id);
            return Json(m);
        }

        //[HttpPost("api/updateUser")]
        //public void updateUser([FromBody] UserInfo user, ModuleInfo module)
        //{
        //    var k = 1488; ;

        //}

        private class ModuleInfo
        {

            public string n { get; set; }

        }

        private class UserInfo
        {
            public string Username { get; set; }
        }



        // TODO реалізувати API для фронтенду, узгодити із Сокуром,
        // який розроблятиме відповідний сервіс на фронті. 
        // Інформація передаватиметься за допомогою json





    }

}



