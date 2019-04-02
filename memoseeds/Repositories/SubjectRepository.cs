
using System;
using System.Collections.Generic;
using memoseeds.Database;
using memoseeds.Models.Entities;
using memoseeds.Repositories.Purchase;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace memoseeds.Repositories
{
    public class SubjectRepository : ISubjectRepository, IRepository<Subject>, IDisposable
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

        public Subject Insert(Subject entity)
        {
            context.Subjects.Add(entity);
            Save();
            return entity;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Subject Update(Subject entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            Save();
            Subject update = context.Subjects.Find(entity.SubjectId);
            return update;

        }

        public Subject GetById(long id)
        {
            return context.Subjects.Find(id);
        }

        public ICollection<Subject> GetWithoutLocalModulesTerms()
        {
            ICollection<Subject> subjects = context.Subjects.Include(d => d.Categories).
            ThenInclude(r => r.Modules)
                .ThenInclude(f => f.Terms).ToList();
            return subjects;
        }

        public ICollection<Subject> GetSubjectsWithCategories()
        {
            ICollection<Subject> subjects = context.Subjects.Include(d => d.Categories).ToList();
            return subjects;
        }

        public ICollection<Subject> GetWithoutLocalModules()
        {
            ICollection<Subject> subjects = context.Subjects.Include(d => d.Categories).
            ThenInclude(r => r.Modules).ToList();
            return subjects;
        }

        public Subject GetSubjectName(string name)
        {
            return context.Subjects.Where(f => f.Name == name).FirstOrDefault();
        }

        public Category GetCategoryName(string name)
        {
            return context.Categories.Where(f => f.Name == name).FirstOrDefault();
        }
    }
}
