using System;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{

    [ApiController]
    public class StudyController:  Controller
    {
        private ModuleRepository moduleRepository;
        private UserRepository userRepository;

            public StudyController(ModuleRepository ModuleRepository, UserRepository userRepository)
        {
            this.moduleRepository = ModuleRepository;
            this.userRepository = userRepository;
        }

        [HttpGet("api/getUser")]
        public JsonResult getUs()
        {
            return Json(userRepository.getUserByName("kovalenko"));
        }

        [HttpPost("api/getModule")]
        public JsonResult getModule([FromBody]long id)
        {
            Module m = moduleRepository.GetById(id);
            return Json(m);
        }

        [HttpPost("api/updateUser")]
        public void updateUser([FromBody] UserInfo user, ModuleInfo module)
        {

        }




        // TODO реалізувати API для фронтенду, узгодити із Сокуром,
        // який розроблятиме відповідний сервіс на фронті. 
        // Інформація передаватиметься за допомогою json
    }

    public class ModuleInfo
    {



    }

    public class UserInfo
    {
        internal object country;

        public string Username { get; set; }
    }


}
