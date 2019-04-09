using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{

    [ApiController]
    [Authorize]
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
        public JsonResult GetModulesOfUser([FromBody] long id)
        {          
            User user = UserRepository.GetById(id);
            List<Module> res = new List<Module>();
            foreach(AquiredModules x in user.Aquireds){
                res.Add(x.Module);
            }
            return (Json(res));
        }

        [HttpPost("api/getModule")]
        public JsonResult GetModule([FromBody]long id)
        {
            Module m = ModuleRepository.GetById(id);
            return Json(m);
        }


        //TODO
        [HttpPost("api/updateUser")]
        public void UpdateUser([FromBody] Info user)
        {

        }




    }

    public class Info
    {
    }
}



