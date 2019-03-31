using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using memoseeds.Models.Entities;
using memoseeds.Repositories;
using memoseeds.Repositories.Purchase;
using Microsoft.AspNetCore.Mvc;

namespace memoseeds.Controllers
{
    [ApiController]
    public class FilterController : Controller
    {
        private IModuleRepository ModuleRepository;
        private ISubjectRepository SubjectRepository;
        private IUserRepository UserRepository;

        public FilterController(IModuleRepository ModuleRepository, ISubjectRepository SubjectRepository, IUserRepository UserRepository)
        {
            this.ModuleRepository = ModuleRepository;
            this.SubjectRepository = SubjectRepository;
            this.UserRepository = UserRepository;
        }

        [HttpPost("/shop/filter")]
        public JsonResult FilterCase([FromBody] Filter cases)
        {
            ICollection<Subject> subjects = SubjectRepository.GetWithoutLocalModules();
            ICollection<Subject> result = new List<Subject>();
            if (!cases.Subject.Equals("default"))
            {
                Subject subject = null;
                foreach (Subject f in subjects)
                    if (f.Name.Equals(cases.Subject))
                    {
                        subject = f;
                        break;
                    }

                 if (!cases.Category.Equals("default"))
                {
                    ICollection<Category> categories = subject.Categories;
                    ICollection<Category> temp = new List<Category>();
                    foreach (Category c in categories)
                    {
                        if (c.Name.Equals(cases.Category))
                            temp.Add(c);
                    }
                    subject.Categories = temp;
                   
                }
                result.Add(subject);
                subjects = result;

            }

            if (cases.IsFree)
            {
                result = new List<Subject>();
                foreach(Subject s in subjects)
                {
                    ICollection<Category> categories = s.Categories;
                    ICollection<Category> tempCategory = new List<Category>();
                    foreach ( Category c in categories)
                    {
                        ICollection<Module> modules = c.Modules;
                        ICollection<Module> temp = new List<Module>();
                        foreach ( Module m in modules)
                        {
                            if (m.Price == 0)
                                temp.Add(m);
                        }

                        if (temp.Count != 0)
                        {
                            c.Modules = temp;
                            tempCategory.Add(c);
                        }
                    }
                    if(tempCategory.Count!=0)
                    {
                        s.Categories = tempCategory;
                        result.Add(s);
                    }
                }
                subjects = result;
            }

            return Json(subjects);
        }

        public class Filter
        {
            [Required(ErrorMessage = "Username not specified")]
            public string Subject { get; set; }

            [Required(ErrorMessage = "Username not specified")]
            public string Category { get; set; }

            [Required(ErrorMessage = "Username not specified")]
            public bool IsFree { get; set; }
        }
    }
}
