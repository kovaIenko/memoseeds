
using System;
using System.Collections.Generic;
using memoseeds.Database;
using memoseeds.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace memoseeds.Repositories
{
    public class UserRepository : IRepository<User>, IDisposable, IUserRepository
    {

        public User GetUserByName(string name)
        {
            return context.Users.Where(h => h.Username.Equals(name)).First();
        }

        public User GetUserByEmail(string email)
        {
            return context.Users.Where(h => h.Email.Equals(email)).First();
        }

        private bool disposed = false;
        private ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Delete(long Id)
        {
           context.Users.Remove(GetById(Id));
        }

        public User GetById(long id)
        {
            return context.Users.Find(id);
        }

        public void Insert(User entity)
        {
            context.Users.Add(entity);
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

        public ICollection<AquiredModules> GetModulesByUser(long id)
        {
            ICollection<AquiredModules> modules = context.AquiredModules.Include(f => f.Module).Where(g=>g.UserId==id).ToList();
            return modules;
        }

        public int NumbOfModules(long id)
        {
            return context.AquiredModules.Where(f => f.UserId == id).Count();
        }


    }
}
