
using memoseeds.Repositories;
using Microsoft.AspNetCore.Mvc;
using memoseeds.Repositories.Purchase;
using memoseeds.Models.Entities;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace memoseeds.Controllers
{
    [ApiController]
    //[Authorize]
    public class AssetsController : Controller
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

        [HttpGet("user/{id}/search/modules/{str}")]
        public IActionResult SearchByModules([FromRoute] long id, [FromRoute] string str)
        {
            IActionResult response = Unauthorized();
            try
            {
                if (str.Length == 0) throw new FormatException();
                ICollection<AquiredModules> myAquiredModules = UserRepository.GetModulesByUserBySubString(id, str);
                ICollection<Module> myModels = ModalsFromAquiredModules(myAquiredModules);
                ICollection<Module> models = ModuleRepository.GetModulesBySubString(str);
                response = Ok(new { MyModels = myModels, NonLocal = models });
            }
            catch (Exception e)
            {
                response =  BadRequest(new { e });
            }
            return response;
        }

        private ICollection<Module> ModalsFromAquiredModules(ICollection<AquiredModules> myModelsUsers)
        {
            ICollection<Module> myModels = new List<Module>();
            foreach (AquiredModules a in myModelsUsers)
                myModels.Add(a.Module);
            return myModels;
        }

        [HttpGet("user/{id}/modules")]
        public IActionResult GetModules([FromRoute] long id)
        {
            IActionResult response = Unauthorized();
            try
            {
                ICollection<AquiredModules> modules = UserRepository.GetModulesByUser(id);
                response = Ok(new { modules });
            }
            catch(Exception e)
            {
                response = BadRequest(new {e});
            }
            return response;
        }

        [HttpPost("user/{userid}/delete/modules/{moduleid}")]
        public IActionResult DeleteUsersModule([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
                AquiredModules aquiredModule = UserRepository.GetAquiredByUserAndModule(userid, moduleid);
                if (aquiredModule == null) throw new NullReferenceException("User doesn't have this module.");
                UserRepository.DeleteUsersModule(aquiredModule);
                ModuleRepository.Delete(aquiredModule.Module);
                response = Ok(new { result = "The module was deleted successfully." });
            }
            catch (Exception e)
            {
                response = BadRequest(new { e });
            }
            return response;
        }

        [HttpGet("user/{userid}/modules/{moduleid}")]
        public IActionResult GetFullModuleByUser([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {

                Module module = UserRepository.GetModuleWithTerms(userid, moduleid);

                long categoryId = (long)module.CategoryId;

                Category category = SubjectRepository.GetCategoryById(categoryId);
                long subjectId = (long)category.SubjectId;
                Subject subject = SubjectRepository.GetSubjectById(subjectId);

                response = Ok(new { Subject = subject.Name, Category = category.Name  , Module = module });
            }
            catch(Exception e)
            {
                response = BadRequest(new { e });
            }
            return response;
        }

        public class UserData
        {
            [Required(ErrorMessage = "Email not specified")]
            public string Email { set; get; }

            [Required(ErrorMessage = "Password not specified")]
            public string Password { set; get; }

        }

    }
}
