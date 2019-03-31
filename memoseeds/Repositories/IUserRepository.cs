using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;

namespace memoseeds.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        User GetUserByName(string name);
        ICollection<AquiredModules> GetModulesByUser(long id);
    }
}
