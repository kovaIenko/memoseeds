using System;
using System.Collections.Generic;
using System.Linq;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("shop")]
    public class ShopController : Controller
    {
        private IModuleRepository ModuleRepository;
        private ISubjectRepository SubjectRepository;
        private IUserRepository UserRepository;

        public ShopController(IModuleRepository ModuleRepository, ISubjectRepository SubjectRepository, IUserRepository UserRepository)
        {
            this.ModuleRepository = ModuleRepository;
            this.SubjectRepository = SubjectRepository;
            this.UserRepository = UserRepository;
        }

        [HttpGet("subjects/categories/modules")]
        public IActionResult GetWithoutLocalModules()
        {
            IActionResult response = Unauthorized();
            try
            {
                ICollection<Subject> subjects = SubjectRepository.GetWithoutLocalModulesTerms();
                Dictionary<string, ICollection<Module>> dictionary = ModulesBySubjects(subjects);
                response = Ok(dictionary);
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        [HttpGet("modules/{moduleid}")]
        public IActionResult GetFullModule([FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
                Module module = ModuleRepository.GetModuleWithTerms(moduleid);
                module.Terms = LeaveNTerms(module.Terms);
                response = Ok(new { module });
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        private ICollection<Term> LeaveNTerms(ICollection<Term> terms)
        {
            int limit = 5;
            if (terms.Count <= limit) return terms;
            ICollection<Term> temps = new List<Term>();
            for (int i = 0; i < limit; i++)
                temps.Add(terms.ElementAt(i));
            return temps;
        }

        [HttpGet("subjects/categories")]
        public IActionResult GetSubjectsWithCategories()
        {
            IActionResult response = Unauthorized();
            try
            {
                ICollection<Subject> subjects = SubjectRepository.GetSubjectsWithCategories();
                response = Ok(value: SubjectsToDictionary(subjects));
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }
            return response;
        }

        [HttpPost("user/{userid}/get/module/{moduleid}")]
        public IActionResult GetModule([FromRoute] long userid, [FromRoute] long moduleid)
        {
            IActionResult response = Unauthorized();
            try
            {
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
            }
            catch (Exception e)
            {
                response = Ok(new { e });
            }

            return response;
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
    }
}
