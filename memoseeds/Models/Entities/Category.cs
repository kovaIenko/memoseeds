using System;
using System.Collections.Generic;

namespace memoseeds.Models.Entities
{
    public class Category
    {
        public int categoryId { get; set;  }
        public String name { get; set; }
        public int subjectId { get; set;  }
        public virtual ICollection<Course> Courses { get; set; }
    }
}