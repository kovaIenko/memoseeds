using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("collector")]
    public class Collector
    {
        [Key]
        [Column("id")]
        public int CollectorId { get; set;  }

        [Column("term")]
        public int TermId { get; set; }
        public Term Term { get; set; }

        [Column("user")]
        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Completion> Completions { get; set; }
    }
}