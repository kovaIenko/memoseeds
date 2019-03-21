using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("types")]
    public class Type
    {
        [Key]
        [Column("id")]
        public int TypeId { get; set;  }

        [Column("type")]
        [Index(IsUnique = true)]
        public TypeOfStudy Name { get; set; }

        public virtual ICollection<Collector> Collectors { get; set; }
    }
}