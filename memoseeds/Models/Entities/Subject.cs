using System;
using System.Collections.Generic;


namespace memoseeds.Models.Entities
{
    public class Subject
    {
        public int subjectId { get; set;}
        public String name { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
