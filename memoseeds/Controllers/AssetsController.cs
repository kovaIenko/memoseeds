
using memoseeds.Repositories;
using Microsoft.AspNetCore.Mvc;
using memoseeds.Repositories.Purchase;
using memoseeds.Models.Entities;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

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
        public JsonResult GetWithoutLocalModules()
        {
            ICollection<Subject> subjects = SubjectRepository.GetWithoutLocalModulesTerms();
            return Json(ModulesBySubjects(subjects));
        }

        [HttpGet("user/{id}/search/modules/{str}")]
        public IActionResult SearchByModules([FromRoute] long id, [FromRoute] string str)
        {
            ICollection<AquiredModules> myAquiredModules = UserRepository.GetModulesByUserBySubString(id, str);
            ICollection<Module> myModels = ModalsFromAquiredModules(myAquiredModules);
            ICollection<Module> models = ModuleRepository.GetModulesBySubString(str);
            return Ok(new { MyModels = myModels, NonLocal  = models});
        }

        private ICollection<Module> ModalsFromAquiredModules(ICollection<AquiredModules> myModelsUsers)
        {
            ICollection<Module> myModels = new List<Module>();
            foreach(AquiredModules a in myModelsUsers)
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
            return Json(module);
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

        [HttpPost("user/create/module")]
        public IActionResult CreateModule([FromBody] ModuleData module)
        {
            IActionResult response = Unauthorized();
            Category category = SubjectRepository.GetCategoryName(module.Category);
            Module created = new Module()
            {
                CategoryId = category.CategoryId,
                InheritedFrom = module.InheritedFrom,
                IsLocal = module.IsLocal,
                Name = module.Name,
                Price = module.Price,
                UserId = module.Author,
               // Terms = GetTermFromIDict(module.Terms)
            };

            ModuleRepository.Insert(created);
            ModuleRepository.Save();

            created.Terms = GetTermFromIDict(module.Terms, created.ModuleId);
            ModuleRepository.Update(created);
            AquiredModules aquiredModules = new AquiredModules()
            {
                UserId = module.Author,
                ModuleId = created.ModuleId
            };
            UserRepository.InsertUserModule(aquiredModules);
            return Ok();
        }


        private ICollection<Term> GetTermFromIDict(IDictionary<string, string> keyValue, long moduleid )
        {
            ICollection<Term> dictionary = new List<Term>();
            foreach(string str in keyValue.Keys)
            {
                dictionary.Add(new Term()
                {
                    //ModuleId = moduleid,
                    Name = str,
                    Definition = keyValue[str],
                });
            }
            return dictionary;
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

        public class UserData
        {
            [Required(ErrorMessage = "Email not specified")]
            public string Email { set; get; }

            [Required(ErrorMessage = "Password not specified")]
            public string Password { set; get; }

        }

        public class ModuleData
        {
            [Required(ErrorMessage = "Author not specified")]
            public long Author { set; get; }
            [Required(ErrorMessage = "Category name not specified")]
            public string Category { set; get; }
            [Required(ErrorMessage = "Inherited id not specified")]
            public long InheritedFrom { set; get; }
            [Required(ErrorMessage = "Name not specified")]
            public string Name { set; get; }
            [Required(ErrorMessage = "Enviroment not specified")]
            public bool IsLocal { set; get; }
            [Required(ErrorMessage = "Price not specified")]
            public int Price { set; get; }
            [Required(ErrorMessage = "Terms not specified")]
            public IDictionary<string, string> Terms { set; get; }
        }
    }
}
