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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ModuleId { get; set; }

        [Required]
        [Column("author")]
        public long AuthorId { get; set;  }

        [Required]
        [Column("is_free")]
        public Boolean IsFree { get; set; }

        [Required]
        [Column("is_local")]
        public Boolean IsLocal { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("category")]
        public long? CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<Term> Terms { get; set; }

        public virtual ICollection<AquiredModules> Aquireds { get; set; }
        public virtual ICollection<VisibleModules> Visibles { get; set; }
    }
}