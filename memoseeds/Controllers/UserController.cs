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

        [HttpPost("user/create/module")]
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
                UserId = module.Author,
            };
        }

        private ICollection<Term> GetTermFromIDict(IDictionary<string, string> keyValue, long moduleid)
        {
            ICollection<Term> dictionary = new List<Term>();
            foreach (string str in keyValue.Keys)
            {
                dictionary.Add(new Term()
                {
                    Name = str,
                    Definition = keyValue[str],
                });
            }
            return dictionary;
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
