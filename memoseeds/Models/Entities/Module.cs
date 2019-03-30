using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace memoseeds.Models.Entities
{
    [Table("modules")]
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long ModuleId { get; set; }

        [Column("author")]
        public long? UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [Required]
        [Column("is_free")]
        public Boolean IsFree { get; set; }

        [Required]
        [Column("is_local")]
        public Boolean IsLocal { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("category")]
        public long? CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }

        [Required]
        public virtual ICollection<Term> Terms { get; set; }

        [JsonIgnore]
        public virtual ICollection<AquiredModules> Aquireds { get; set; }
        [JsonIgnore]
        public virtual ICollection<VisibleModules> Visibles { get; set; }
    }
}