using System;
using System.Collections.Generic;

namespace memoseeds.Models.Entities
{
    public class User
    {
        public int userId { get; set; }
        public int courseId { get; set; }
        public bool isLocal { get; set; }
        public virtual ICollection<Collector> collectors { get; set; }
    }
}