using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("completions")]
    public class Completion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long CompletionId { get; set; }

        [Column("collector")]
        public long? CollectorId { get; set; }
        public Collector Collector { get; set; }

        [Column("type")]
        public long? TypeId { get; set; }
        public Type Type { get; set; }

        [Required]
        [Column("success")]
        public int NumSuccess { get; set; }

        [Required]
        [Column("attempt")]
        public int NumAttempt { get; set; }
    }
}
