using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("aquired_modules")]
    public class AquiredModules
    {
        [Key]
        [Column("id")]
        public int AquiredModulesId { get; set; }

        [Column("user")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("module")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

    }
}
