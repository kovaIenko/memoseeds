using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("modules")]
    public class Module
    {
        [Key]
        [Column("id")]
        public int ModuleId { get; set; }

        [Required]
        [Column("author")]
        public int AuthorId { get; set;  }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("course")]
        public int? CourseId { get; set; }
        public Course Course { get; set; }

        public virtual ICollection<Term> Terms { get; set; }
    }
}