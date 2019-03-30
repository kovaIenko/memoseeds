
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

        public AssetsController(IModuleRepository ModuleRepository)
        {
            this.ModuleRepository = ModuleRepository;
        }

        [HttpGet("public/modules")]
        public JsonResult getAllPublicModules()
        {
            ICollection<Module> modules = ModuleRepository.GetPublicModules();
            return Json(modules);
        }


    }
}
