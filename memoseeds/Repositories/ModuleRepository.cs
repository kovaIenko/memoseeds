
using System;
using System.Collections.Generic;
using memoseeds.Database;
using memoseeds.Models.Entities;
using memoseeds.Repositories.Purchase;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace memoseeds.Repositories
{
    public class ModuleRepository : IRepository<Module>, IDisposable, IModuleRepository

    {
        private bool disposed = false;
        private ApplicationDbContext context;

        public ModuleRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public Module GetById(long id)
        {
            /* треба протестувати*/
            return context.Modules.Include(g => g.Terms).FirstOrDefault(d => d.ModuleId == id);
        }

        public ICollection<Module> GetPublicModules()
        {
            ICollection<Module> modules = context.Modules.Include(e=>e.Category).Include(n => n.Terms).ToList();
            return modules;
        }


        public Module Insert(Module entity)
        {
            context.Modules.Add(entity);
            Save();
            return entity;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Module Update(Module entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            Save();
            Module updated = context.Modules.Find(entity.ModuleId);
            return updated;
        }

        public void Delete(Module entity)
        {
            context.Modules.Remove(entity);
            context.SaveChanges();
        }

        public ICollection<Module> GetNonLocalModules()
        {
            ICollection<Module> modules = context.Modules.Where(d => !d.IsLocal).Include(e => e.Terms).Include(d=>d.Category).ThenInclude(w=>w.Subject).ToList();
            return modules;
        }

        public Module GetModuleWithTerms(long moduleid)
        {
            Module module = context.Modules.Include(r => r.Terms).FirstOrDefault(g => g.ModuleId == moduleid); 
            return module;
        }

        public ICollection<Module> GetWithoutLocalWithTerms()
        {
            ICollection<Module> modules = context.Modules.Include(h => h.Terms).Where(d => !d.IsLocal).ToList();
            return modules;
        }

        public ICollection<Module> GetModulesBySubString(string str)
        {
            ICollection<Module> modules = context.Modules.Include(h => h.Terms).Where(d => !d.IsLocal).Where(k => k.Name.Contains(str)).ToList();
            return modules;
        }

       
    }
}
