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
        private IModuleRepository ModuleRepository;
        private IUserRepository UserRepository;

            public StudyController(IModuleRepository ModuleRepository,IUserRepository UserRepository)
        {
            this.ModuleRepository = ModuleRepository;
            this.UserRepository = UserRepository;
        }

        [HttpGet("api/getUsers")]
        public JsonResult getUs()
        {
            return Json(UserRepository.getAllUsers());
        }

        [HttpPost("api/getModule")]
        public JsonResult getModule([FromBody]long id)
        {
            Module m = ModuleRepository.GetById(id);
            return Json(m);
        }

        [HttpPost("api/updateUser")]
        public void updateUser([FromBody] Info user)
        {
            var k = 1488; ;

        }





        // TODO реалізувати API для фронтенду, узгодити із Сокуром,
        // який розроблятиме відповідний сервіс на фронті. 
        // Інформація передаватиметься за допомогою json





    }

    public class Info
    {
    }
}



