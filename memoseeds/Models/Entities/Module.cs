using System;
using System.Collections.Generic;


namespace memoseeds.Models.Entities
{
    public class Module
    {
        public Module()
        { }
        public int authorId { get; set;  }
        public int moduleId { get; set; }
        public String name { get; set; }
        public virtual ICollection<Term> Terms { get; set; }
    }
}