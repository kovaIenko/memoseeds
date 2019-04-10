using System;
using System.Collections.Generic;
using System.Linq;
using memoseeds.Models;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    [ApiController]
    [Authorize]
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
                ICollection<Subject> subjects = SubjectRepository.GetModulesTerms();
                response = Ok(ModulesBySubjects(subjects));
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
                // додати перевірку на наявність такого модуля
                Module module = ModuleRepository.GetModuleWithTerms(moduleid);
                long categoryId = (long)module.CategoryId;
                Category category = SubjectRepository.GetCategoryById(categoryId);
                long subjectId = (long)category.SubjectId;
                Subject subject = SubjectRepository.GetSubjectById(subjectId);
                int countTerms = module.Terms.Count;
                module.Terms = LeaveNTerms(module.Terms);
                response = Ok(new { Subject = subject.Name, Category = category.Name, Module = module, NumbTerms = countTerms });
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
                        if (!m.IsLocal)
                            map[s.Name].Add(m);
                }
            }
            return map;
        }
    }
}
