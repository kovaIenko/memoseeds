using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;

namespace memoseeds.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByName(string name);
        ICollection<AquiredModules> GetModulesByUser(long id);
        int NumbOfModules(long id);
        User GetUserByEmail(string email);
        Module GetModuleWithTerms(long userId, long moduleId);
        bool UserHasModel(long userId, long moduleId);
    }
}