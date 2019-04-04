using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
        Byte[] getUserImage(long id);
        ICollection<AquiredModules> GetModulesByUserBySubString(long id, string str);
        AquiredModules InsertUserModule(AquiredModules entity);
        void DeleteUsersModule(AquiredModules entity);
        AquiredModules GetAquiredByUserAndModule(long userId, long moduleId);
        
    }
}