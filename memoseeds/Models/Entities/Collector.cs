using System;
using System.Collections.Generic;

namespace memoseeds.Models.Entities
{
    public class Collector
    {
        public int collectorId { get; set;  }
        public int termId { get; set; }
        public int userId { get; set; }
        public virtual ICollection<Term> terms { get; set; }
        public virtual ICollection<User> users { get; set; }
        public virtual ICollection<Type> types { get; set; }
    }
}