using System;
using System.Collections.Generic;


namespace memoseeds.Models.Entities
{
    public class Subject
    {
        public Subject()
        { }
        public int subjectId { get; set;}
        public String name { get; set; }
        public virtual ICollection<Category> categories { get; set; }

    }
}
