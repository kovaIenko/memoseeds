using System;
namespace memoseeds.Repositories
{
    public interface IRepository<T> : IDisposable
    {

        T GetById(long id);
        T Insert(T entity);
        void Delete(long Id);
        T Update(T entity);
        void Save(); 
    }
}
