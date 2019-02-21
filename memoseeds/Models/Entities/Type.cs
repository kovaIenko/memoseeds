using System;
using System.Collections.Generic;

namespace memoseeds.Models.Entities
{
    public class Type
    {
        public int typeId { get; set;  }
        public String name { get; set; }
        public virtual ICollection<Collector> collectors { get; set; }
    }
}