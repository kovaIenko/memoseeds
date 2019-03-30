using System;
using memoseeds.Database;
using memoseeds.Models.Entities;

namespace memoseeds.Repositories
{
    public class UserRepository : IRepository<User>, IDisposable
    {


        public User getUserByName(string name)
        {
            return context.Users.Find(name);
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

        public void Update(User entity)
        {
            context.Users.Update(entity);
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



}
}
