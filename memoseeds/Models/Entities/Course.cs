using System;
using System.Collections.Generic;

namespace memoseeds.Models.Entities
{
    public class Course
    {
        public int categoryId { get; set;  }
        public int courseId { get; set; }
        public String name { get; set; }
        public Boolean isDefault { get; set;  }
        public Boolean isFree { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}