using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;

namespace memoseeds.Repositories
{
    public interface IUserRepository: IRepository<User>
    {

         User getUserByName(string name);



    }
}
