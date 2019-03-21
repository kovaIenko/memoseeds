using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("terms")]
    public class Term
    {
        [Key]
        [Column("id")]
        public int TermId { get; set;  }

        [Column("type")]
        public string Name { get; set; }

        [Column("module")]
        public int? ModuleId { get; set; }
        public Module Module { get; set; }

        [Column("definition")]
        public string Definition { get; set; }
    }
}