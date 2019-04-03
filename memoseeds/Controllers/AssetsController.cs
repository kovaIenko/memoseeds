
using memoseeds.Repositories;
using Microsoft.AspNetCore.Mvc;
using memoseeds.Repositories.Purchase;
using memoseeds.Models.Entities;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace memoseeds.Controllers
{
    [ApiController]
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

        [HttpGet("shop/subjects/categories/modules")]
        public IActionResult GetWithoutLocalModules()
        {
            IActionResult response = Unauthorized();
            try
            {
                ICollection<Subject> subjects = SubjectRepository.GetWithoutLocalModulesTerms();
                response = Ok(ModulesBySubjects(subjects));
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;

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
                return response;
            }
            catch (Exception e)
            {
                return Ok(new { e });
            }
        }

        private ICollection<Module> ModalsFromAquiredModules(ICollection<AquiredModules> myModelsUsers)
        {
            ICollection<Module> myModels = new List<Module>();
            foreach (AquiredModules a in myModelsUsers)
                myModels.Add(a.Module);
            return myModels;
        }

        [HttpGet("user/{id}/modules")]
        public JsonResult GetModules([FromRoute] long id)
        {
            ICollection<AquiredModules> modules = UserRepository.GetModulesByUser(id);
            return Json(modules);
        }

        [HttpGet("user/{userid}/modules/{moduleid}")]
        public JsonResult GetFullModuleByUser([FromRoute] long userid, [FromRoute] long moduleid)
        {
            Module module = UserRepository.GetModuleWithTerms(userid, moduleid);
            return Json(module);
        }

        [HttpGet("shop/modules/{moduleid}")]
        public JsonResult GetFullModule([FromRoute] long moduleid)
        {
            Module module = ModuleRepository.GetModuleWithTerms(moduleid);
            module.Terms = LeaveNTerms(module.Terms);
            return Json(module);
        }

        private ICollection<Term> LeaveNTerms(ICollection<Term> terms)
        {
            ICollection<Term> temps = new List<Term>();
            for (int i = 0; i < 5; i++)
                temps.Add(terms.ElementAt(i));
            return temps;
        }

        [HttpGet("shop/subjects/categories")]
        public JsonResult GetSubjectsWithCategories()
        {
            ICollection<Subject> subjects = SubjectRepository.GetSubjectsWithCategories();
            return Json(SubjectsToDictionary(subjects));
        }

        [HttpPost("shop/user/{userid}/get/module/{moduleid}")]
        public IActionResult GetModule([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            if (IsExist(userid, moduleid)) return Ok(new { result = "User has this module." });
            Module module = ModuleRepository.GetById(moduleid);
            int moduleCost = module.Price;
            User user = UserRepository.GetById(userid);
            if (user.Credits > moduleCost)
            {
                Module copied = Copy(module);
                user.Credits -= moduleCost;
                //має працювати
                user.Aquireds.Add(new AquiredModules() { LastEdit = DateTime.Now, Module = copied, User = user });
                UserRepository.Update(user);
                UserRepository.Save();
                response = Ok(new { result = "You buy module successfully." });
            }
            else response = Ok(new { result = "Not enough credits." });
            return response;
        }


        private Dictionary<string, ICollection<Module>> ModulesBySubjects(ICollection<Subject> subjects)
        {
            Dictionary<string, ICollection<Module>> map = new Dictionary<string, ICollection<Module>>();
            foreach (Subject s in subjects)
            {
                map.Add(s.Name, new List<Module>());
                ICollection<Category> categories = s.Categories;
                foreach (Category c in categories)
                {
                    ICollection<Module> modules = c.Modules;
                    foreach (Module m in modules)
                        map[s.Name].Add(m);
                }
            }
            return map;
        }

        /* check if user has this module */
        private bool IsExist(long useid, long moduleid)
        {
            bool userHas = UserRepository.UserHasModel(useid, moduleid);
            return userHas;
        }

        private Module Copy(Module module)
        {
            Module copy = module;
            copy.IsLocal = true;
            copy.InheritedFrom = module.ModuleId;
            copy.Price = 0;
            return copy;
        }

        private Dictionary<string, ICollection<string>> SubjectsToDictionary(ICollection<Subject> subjects)
        {
            Dictionary<string, ICollection<string>> map = new Dictionary<string, ICollection<string>>();
            foreach (Subject s in subjects)
            {
                map.Add(s.Name, new List<string>());
                ICollection<Category> categories = s.Categories;
                foreach (Category c in categories)
                    map[s.Name].Add(c.Name);
            }
            return map;
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
