using System;
namespace memoseeds.Repositories
{
    public interface IRepository<T> : IDisposable
    {

        T GetById(long id);
        T Insert(T entity);
        void Delete(T entity);
        T Update(T entity);
        void Save(); 
    }
}
