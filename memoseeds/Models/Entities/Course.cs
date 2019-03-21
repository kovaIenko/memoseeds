using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{

    [Table("courses")]
    public class Course
    {
        [Key]
        [Column("id")]
        public int CourseId { get; set; }

        [Column("category")]
        public int? CategoryId { get; set;  }
        public Category Category { get; set; }

        [Column("name")]
        public String Name { get; set; }

        [Required]
        [Column("is_default")]
        public Boolean IsDefault { get; set;  }

        [Required]
        [Column("is_free")]
        public Boolean IsFree { get; set; }

        [Column("user")]
        public int UserId { get; set; }

        public virtual ICollection<Module> Modules { get; set; }

        public virtual ICollection<AquiredCourses> Aquireds { get; set; }
        public virtual ICollection<VisibleCourses> Visibles { get; set; }
    }
}