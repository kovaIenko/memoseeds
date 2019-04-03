using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("visible_modules")]
    public class VisibleModules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long VisibleModulesId { get; set; }

        [Column("user")]
        public long? UserId { get; set; }
        public User User { get; set; }

        [Column("module")]
        public long? ModuleId { get; set; }
        public Module Module { get; set; }

        [Column("can_edit")]
        public Boolean CanEdit { get; set; }
    }
}
