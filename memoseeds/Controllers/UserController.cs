using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
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
                Module created = CreateModule(module);
                ModuleRepository.Insert(created);

                created.Terms = GetTermFromIDict(module.Terms, created.ModuleId);
                Module added = ModuleRepository.Update(created);
                DateTime current = System.DateTime;
                AquiredModules aquiredModules = new AquiredModules()
                {
                    UserId = module.Author,
                    ModuleId = created.ModuleId,
                    LastEdit =
                };

                UserRepository.InsertUserModule(aquiredModules);
            }catch(Exception e)
            {

            }

            }

            private Module CreateModule(ModuleData obj)
            {
                return new Module()
                {
                    CategoryId = category.CategoryId,
                    InheritedFrom = module.InheritedFrom,
                    IsLocal = module.IsLocal,
                    Name = module.Name,
                    Price = module.Price,
                    UserId = module.Author,
                    // Terms = GetTermFromIDict(module.Terms)
                };

            }


            private ICollection<Term> GetTermFromIDict(IDictionary<string, string> keyValue, long moduleid)
            {
                ICollection<Term> dictionary = new List<Term>();
                foreach (string str in keyValue.Keys)
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
