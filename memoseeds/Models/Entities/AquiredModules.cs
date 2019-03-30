using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("aquired_modules")]
    public class AquiredModules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long AquiredModulesId { get; set; }

        [Column("user")]
        public long? UserId { get; set; }
        public User User { get; set; }

        [Column("module")]
        public long? ModuleId { get; set; }
        public Module Module { get; set; }

    }
}
