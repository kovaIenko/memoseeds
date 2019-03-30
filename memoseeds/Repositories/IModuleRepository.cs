using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;

namespace memoseeds.Repositories.Purchase
{
    public interface IModuleRepository
    {
        ICollection<Module> GetPublicModules();
    }
}
