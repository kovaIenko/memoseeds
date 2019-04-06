
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
        private const bool V = false;
        private bool disposed = false;
        private ApplicationDbContext context;

        public SubjectRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Delete(Subject entity)
        { 
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

        ICollection<Subject> ISubjectRepository.GetModulesTerms()
        {
             ICollection<Subject> subjects = context.Subjects.Include(d => d.Categories).ThenInclude(e => e.Modules).ThenInclude(d => d.Terms).ToList();
            //IEnumerable<Subject> scoreQuery =
            //from s in context.Subjects.Include(d=>d.Categories)
            //join c in context.Categories.Include(s=>s.Modules) on s.SubjectId equals c.SubjectId
            //join m in context.Modules.Include(e=>e.Terms on c.CategoryId equals m.CategoryId
            //where m.IsLocal == false
            //select s;
          //  ICollection<Subject> subjects = scoreQuery.ToList();
            

            return subjects;
        }

        public ICollection<Subject> GetSubjectsWithCategories()
        {
            ICollection<Subject> subjects = context.Subjects.Include(d => d.Categories).ToList();
            return subjects;
        }

        public ICollection<Subject> GetModules()
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
