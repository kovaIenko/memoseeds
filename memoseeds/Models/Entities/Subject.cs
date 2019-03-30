using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("subjects")]
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long SubjectId { get; set;}

        [Required]
        [Index(IsUnique = true)]
        [Column("name")]
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
