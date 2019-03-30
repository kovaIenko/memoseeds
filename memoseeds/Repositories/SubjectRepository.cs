using System;
using memoseeds.Database;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using memoseeds.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace memoseeds.Repositories
{
    public class SubjectRepository : IRepository<Subject>, IDisposable
    {
        private bool disposed = false;
        private ApplicationDbContext context;

        public SubjectRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Delete(long subjectId)
        {
            Subject entity = context.Subjects.Find(subjectId);
            context.Subjects.Remove(entity);
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

        public void Insert(Subject entity)
        {
            context.Subjects.Add(entity);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Subject entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public Subject GetById(long id)
        {
            return context.Subjects.Find(id);
        }
    }
}
