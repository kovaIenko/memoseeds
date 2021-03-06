﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public IActionResult FilterCase([FromBody] Filter cases)
        {
            IActionResult response = Unauthorized();

            string _default = "default";
            try
            {
                ICollection<Subject> subjects = SubjectRepository.GetModulesTerms();
                subjects = ConfigModules.DeleteLocalModules(subjects);

                ICollection<Subject> result = new List<Subject>();
                if (!cases.Subject.Equals(_default))
                {
                    Subject subject = ChooseSubject(subjects, cases.Subject, cases.Category, _default);
                    if (subject == null) return Ok(new List<Subject>());
                    result.Add(subject);
                    subjects = result;
                }
                else response = Ok(new { subjects });

                if (cases.IsFree)
                    subjects = CheckIsFree(subjects);
                response = Ok(subjects);
            }
            catch (Exception e)
            {
                response = BadRequest(new { e });
            }
            return response;
        }

        private Subject ChooseSubject(ICollection<Subject> subjects, string sub, string cat, string _default)
        {
            Subject subject = null;
            foreach (Subject f in subjects)
                if (f.Name.Equals(sub))
                {
                    subject = f;
                    break;
                }

            if (subject != null && !cat.Equals(_default))
                subject = ChooseCategory(subject, cat);

            return subject;
        }

        private Subject ChooseCategory(Subject subject, string category)
        {
            ICollection<Category> categories = subject.Categories;
            ICollection<Category> temp = new List<Category>();

            foreach (Category c in categories)
            {
                if (c.Name.Equals(category))
                {
                    temp.Add(c);
                    break;
                }
            }

            subject.Categories = temp;
            return subject;
        }

        private ICollection<Subject> CheckIsFree(ICollection<Subject> subjects)
        {
            ICollection<Subject> result = new List<Subject>();
            foreach (Subject s in subjects)
            {
                ICollection<Category> categories = s.Categories;
                ICollection<Category> tempCategory = new List<Category>();
                foreach (Category c in categories)
                {
                    ICollection<Module> modules = c.Modules;
                    ICollection<Module> temp = new List<Module>();
                    foreach (Module m in modules)
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
                if (tempCategory.Count != 0)
                {
                    s.Categories = tempCategory;
                    result.Add(s);
                }
            }
            return result;
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
