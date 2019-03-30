
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
            return context.Modules.Find(id);
        }

        public ICollection<Module> GetPublicModules()
        {
            ICollection<Module> modules = context.Modules.Include(n => n.Terms).ToList();
            return modules;
        }

        public void Insert(Module entity)
        {
            context.Modules.Add(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Module entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(long Id)
        {
            Module entity = context.Modules.Find(Id);
            context.Modules.Remove(entity);
        }

    
    }
}
