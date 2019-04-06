using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private IModuleRepository ModuleRepository;
        private ISubjectRepository SubjectRepository;
        private IUserRepository UserRepository;

        public UserController(IModuleRepository ModuleRepository, ISubjectRepository SubjectRepository, IUserRepository UserRepository)
        {
            this.ModuleRepository = ModuleRepository;
            this.SubjectRepository = SubjectRepository;
            this.UserRepository = UserRepository;
        }

        [HttpPost("create/module")]
        public IActionResult CreateModule([FromBody] ModuleData module)
        {
            IActionResult response = Unauthorized();
            try
            {
                Category category = SubjectRepository.GetCategoryName(module.Category);
                Module created = CreateModule(module, category.CategoryId);
                ModuleRepository.Insert(created);

                created.Terms = GetTermFromIDict(module.Terms, created.ModuleId);
                Module added = ModuleRepository.Update(created);
                AquiredModules aquiredModules = CreateUserModule(module.Author, created.ModuleId);

                UserRepository.InsertUserModule(aquiredModules);
                response = Ok(new { moduleId = added.ModuleId });
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        [HttpPost("{userid}/has/module/{moduleid}")]
        public IActionResult UserHasModule([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
                bool userHasModule = IsExist(userid, moduleid);
                response = Ok(new { result = userHasModule });
            }
            catch (Exception e)
            {
                response = BadRequest(new { e });
            }
            return response;
        }

        [HttpPost("{userid}/resources/module/{moduleid}")]
        public IActionResult UserHasEnoughMoney([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
                Module module = ModuleRepository.GetById(moduleid);
                int moduleCost = module.Price;
                User user = UserRepository.GetById(userid);
                response = Ok(new { result = (user.Credits >= moduleCost) });
            }
            catch (Exception e)
            {
                response = BadRequest(new { e });
            }
            return response;
        }


        [HttpPost("{userid}/get/module/{moduleid}")]
        public IActionResult GetModule([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {

                 if (IsExist(userid, moduleid)) return Ok(new { result = "User has this module." });
                 Module module = ModuleRepository.GetById(moduleid);
                int moduleCost = module.Price;

                User user = UserRepository.GetById(userid);

                if (user.Credits >= moduleCost)
                {
                    Module copied = Copy(module);
                    user.Credits -= moduleCost;
                  
                    ModuleRepository.Insert(copied);
                    copied.Terms = CopyTerms(module.Terms, copied.ModuleId);
                    //має працювати
                    Module added = ModuleRepository.Update(copied);

                    AquiredModules aquiredModules = CreateUserModule(userid, added.ModuleId);
                    UserRepository.InsertUserModule(aquiredModules);
                    response = Ok(new { result = "success", moduleId = copied.ModuleId });
                }
                else response = Ok(new { result = "Not enough credits." });
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        /* check if user has this module */
        private bool IsExist(long useid, long moduleid)
        {
            bool userHas = UserRepository.UserHasModel(useid, moduleid);
            return userHas;
        }

        private Module Copy(Module module)
        {
            return new Module()
            {
                CategoryId = module.CategoryId,
                Name = module.Name,
                ModuleId = default(long),
                UserId = module.UserId,
                IsLocal = true,
                InheritedFrom = module.ModuleId,
                Price = 0
            };
        }

        private AquiredModules CreateUserModule(long userid, long moduleid)
        {
            return new AquiredModules()
            {
                UserId = userid,
                ModuleId = moduleid,
                LastEdit = DateTime.Now
            };
        }

        private Module CreateModule(ModuleData module, long categoryId)
        {
            if (!module.IsLocal && module.Category == "default") throw new FormatException("Non-local module must contain a category!");
            return new Module()
            {
                CategoryId = categoryId,
                InheritedFrom = module.InheritedFrom,
                IsLocal = module.IsLocal,
                Name = module.Name,
                Price = module.Price,
                UserId = module.Author
            };
        }

        private ICollection<Term> GetTermFromIDict(IDictionary<string, string> keyValue, long moduleid)
        {
            ICollection<Term> dictionary = new List<Term>();
            foreach (string str in keyValue.Keys)
            {
                dictionary.Add(new Term()
                {
                    ModuleId = moduleid,
                    Name = str,
                    Definition = keyValue[str],
                });
            }
            return dictionary;
        }

        public ICollection<Term> CopyTerms(ICollection<Term> terms, long moduleId)
        {
            ICollection<Term> coppied = new List<Term>();
            foreach (Term t in terms)
            {
                coppied.Add(new Term()
                {
                    ModuleId = moduleId,
                    Name = t.Name,
                    Definition =t.Definition
                });
            }
            return coppied;
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
