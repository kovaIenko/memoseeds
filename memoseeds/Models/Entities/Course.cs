using System;
using System.Collections.Generic;

namespace memoseeds.Models.Entities
{
    public class Course
    {
        public int categoryId { get; set;  }
        public int courseId { get; set; }
        public String name { get; set; }
        public Boolean is_default { get; set;  }
        public Boolean is_free { get; set; }
        public virtual ICollection<Module> modules { get; set; }
    }
}