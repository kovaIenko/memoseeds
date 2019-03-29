using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{

    [Table("categories")]
    public class Category
    {
        [Key]
        [Column("id")]
        public int CategoryId { get; set; }

        [Column("subject")]
        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Column("name")]
        public String Name { get; set; }

        [Column("user")]
        public int UserId { get; set; }

        public virtual ICollection<Module> Modules { get; set; }

    }
}