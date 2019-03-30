
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

        public AssetsController(IModuleRepository ModuleRepository, ISubjectRepository SubjectRepository)
        {
            this.ModuleRepository = ModuleRepository;
            this.SubjectRepository = SubjectRepository;
        }

        [HttpGet("shop/subjects/categories/modules")]
        public JsonResult GetWithoutLocalModules()
        {
            ICollection<Subject> subjects = SubjectRepository.GetWithoutLocalModules();
            return Json(subjects);
        }



    }
}
