
using memoseeds.Repositories;
using Microsoft.AspNetCore.Mvc;
using memoseeds.Repositories.Purchase;
using memoseeds.Models.Entities;
using System.Collections.Generic;

namespace memoseeds.Controllers
{
    [ApiController]
    public class AssetsController: Controller
    {
        private IModuleRepository ModuleRepository;
        private ISubjectRepository SubjectRepository;
        private IUserRepository UserRepository;

        public AssetsController(IModuleRepository ModuleRepository, ISubjectRepository SubjectRepository, IUserRepository UserRepository)
        {
            this.ModuleRepository = ModuleRepository;
            this.SubjectRepository = SubjectRepository;
            this.UserRepository = UserRepository;
        }

        [HttpGet("shop/subjects/categories/modules")]
        public JsonResult GetWithoutLocalModules()
        {
            ICollection<Subject> subjects = SubjectRepository.GetWithoutLocalModules();
            return Json(subjects);
        }

        [HttpGet("user/{id}/modules")]
        public JsonResult GetModules([FromRoute] long id)
        {
            ICollection<AquiredModules> modules = UserRepository.GetModulesByUser(id);
            return Json(modules);
        }

    }
}
