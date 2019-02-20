using System;

namespace memoseeds.Models.Entities
{
    public class Category
    {
        public int categoryId { get; set;  }
        public String name { get; set; }
        public int subjectId { get; set;  }
    }
}