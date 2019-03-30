using System;
namespace memoseeds.Repositories
{
    public interface IRepository<T> : IDisposable
    {

        T GetById(long id);
        void Insert(T entity);
        void Delete(long Id);
        void Update(T entity);
        void Save(); 
    }
}
