using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace memoseeds.Models.Entities
{
    [Table("aquired_modules")]
    public class AquiredModules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long AquiredModulesId { get; set; }

        [JsonIgnore]
        [Column("user")]
        public long? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [Column("module")]
        [JsonIgnore]
        public long? ModuleId { get; set; }
        public Module Module { get; set; }

        [Column("last_edit")]
        public DateTime LastEdit { get; set; }

    }
}
