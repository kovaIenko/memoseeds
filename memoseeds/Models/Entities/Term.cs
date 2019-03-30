using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace memoseeds.Models.Entities
{
    [Table("terms")]
    public class Term
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long TermId { get; set;  }

        [Column("name")]
        public string Name { get; set; }

        [Column("module")]
        public long? ModuleId { get; set; }
        [JsonIgnore]
        public Module Module { get; set; }

        [Column("definition")]
        public string Definition { get; set; }
    }
}