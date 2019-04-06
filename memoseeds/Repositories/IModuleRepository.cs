﻿using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;

namespace memoseeds.Repositories.Purchase
{
    public interface IModuleRepository: IRepository<Module>
    {
        ICollection<Module> GetPublicModules();
        Module GetModuleWithTerms(long moduleid);
        ICollection<Module> GetWithoutLocalWithTerms();
        ICollection<Module> GetModulesBySubString(string str);
        ICollection<Module> GetNonLocalModules();
    }
}
