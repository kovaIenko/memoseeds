using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("visible_modules")]
    public class VisibleModules
    {
        [Key]
        [Column("id")]
        public int VisibleModulesId { get; set; }

        [Column("user")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("module")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        [Column("can_edit")]
        public Boolean CanEdit { get; set; }
    }
}
