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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long TypeId { get; set;  }

        [Column("type")]
        public TypeOfStudy Name { get; set; }

        public virtual ICollection<Collector> Collectors { get; set; }
    }
}