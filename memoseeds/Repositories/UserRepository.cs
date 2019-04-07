
using System;
using System.Collections.Generic;
using memoseeds.Database;
using memoseeds.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata;

namespace memoseeds.Repositories
{
    public class UserRepository : IRepository<User>, IDisposable, IUserRepository
    {
        public User GetUserByName(string name)
        {
            return context.Users.Where(h => h.Username.Equals(name)).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return context.Users.Where(h => h.Email.Equals(email)).FirstOrDefault();
        }

        private bool disposed = false;
        private ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Delete(User entity)
        {
            context.Users.Remove(entity);
        }

        public User GetById(long id)
        {
            return context.Users.Find(id);
        }

        public User Insert(User entity)
        {
            context.Users.Add(entity);
            Save();
            return entity;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public User Update(User entity)
        {
            context.Users.Update(entity);
            Save();
            User update = context.Users.Find(entity.UserId);
            return update;
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

        public ICollection<User> GetAllUsers()
        {
            ICollection<User> res = context.Users.ToList();
            return res;
        }


        public bool UserHasModel(long userId, long moduleId)
        {
            AquiredModules module = context.AquiredModules.Where(d => d.UserId == userId && (d.ModuleId == moduleId || d.Module.InheritedFrom==moduleId)).
            FirstOrDefault();

            return module != null;
        }

        public ICollection<AquiredModules> GetModulesByUser(long id)
        {
            ICollection<AquiredModules> modules = context.AquiredModules.Include(f => f.Module).ThenInclude(r=>r.Terms).Where(g=>g.UserId==id).ToList();
            return modules;
        }

        public ICollection<AquiredModules> GetModulesByUserBySubString(long id, string str)
        {
            ICollection<AquiredModules> modules = context.AquiredModules.Include(f => f.Module).
            ThenInclude(r => r.Terms).Where(k => k.Module.Name.IndexOf(str, StringComparison.OrdinalIgnoreCase) != -1).Where(g => g.UserId == id).ToList();
            return modules;
        }


        public Module GetModuleWithTerms(long userId, long moduleId)
        {
            AquiredModules aquiredModule = context.AquiredModules.Include(r=>r.Module).ThenInclude(y=>y.Terms).Where(q => q.ModuleId == moduleId).FirstOrDefault(g => g.UserId == userId);
            return aquiredModule.Module;
        }

        public int NumbOfModules(long id)
        {
            return context.AquiredModules.Where(f => f.UserId == id).Count();
        }

        public AquiredModules InsertUserModule(AquiredModules entity)
        {
             context.AquiredModules.Add(entity);
             context.SaveChanges();
             return entity;
        }

        public Byte[] getUserImage(long id)
        {
            return GetById(id).Img;
        }

        public void DeleteUsersModule(AquiredModules entity)
        {
            context.AquiredModules.Remove(entity);
            context.SaveChanges();
        }

        public AquiredModules GetAquiredByUserAndModule(long userId, long moduleId)
        {
            return context.AquiredModules.Include(d=>d.Module).FirstOrDefault(f=>(f.UserId==userId&&f.ModuleId==moduleId));
        }
    }
}
