using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("collectors")]
    public class Collector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long CollectorId { get; set;  }

        [Column("term")]
        public long?TermId { get; set; }
        public Term Term { get; set; }

        [Column("user")]
        public long?UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Completion> Completions { get; set; }
    }
}