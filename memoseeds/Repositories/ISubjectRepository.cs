using System;
using System.Collections.Generic;
using memoseeds.Models.Entities;

namespace memoseeds.Repositories
{
    public interface ISubjectRepository : IDisposable
    {

        ICollection<Subject> GetWithoutLocalModules();
        ICollection<Subject> GetWithoutLocalModulesTerms();
    }
}
