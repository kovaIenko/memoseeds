using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;

namespace memoseeds.Models
{
    public static class ConfigModules
    {

        public static ICollection<Subject> DeleteLocalModules(ICollection<Subject> subjects)
        {
            ICollection<Subject> subjs = new List<Subject>();
            foreach (Subject s in subjects)
            {
                ICollection<Category> categories = new List<Category>();
                ICollection<Category> cs = s.Categories;
                foreach (Category c in cs)
                {
                    ICollection<Module> md = c.Modules;
                    ICollection<Module> modules = new List<Module>();
                    foreach (Module m in md)
                        if (!m.IsLocal)
                            modules.Add(m);

                    if (modules.Count != 0)
                    {
                        Category temp = c;
                        temp.Modules = modules;
                        categories.Add(temp);
                    }
                }
                if (categories.Count != 0)
                {
                    Subject temp = s;
                    temp.Categories = categories;
                    subjs.Add(temp);
                }

            }
            return subjs;
        }

    }
}
