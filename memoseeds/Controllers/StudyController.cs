using System;
using System.Collections.Generic;
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


        [HttpPost("api/getModulesOfUser")]
        public JsonResult getModulesOfUser([FromBody] long id)
        {
            
            User user = UserRepository.GetById(id);
            List<Module> res = new List<Module>();
            foreach(AquiredModules x in user.Aquireds){
                res.Add(x.Module);
            }
            return (Json(res));
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


        //TODO
        [HttpPost("api/updateUser")]
        public void updateUser([FromBody] Info user)
        {

        }




    }

    public class Info
    {
    }
}



