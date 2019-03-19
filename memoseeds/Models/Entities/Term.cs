using System;
using System.Collections.Generic;

namespace memoseeds.Models.Entities
{
    public class Term
    {
        public int termId { get; set;  }
        public String name { get; set; }
        public int moduleId { get; set;  }
        public String definition { get; set; }
        public virtual ICollection<Collector> Collectors { get; set; }
    }
}