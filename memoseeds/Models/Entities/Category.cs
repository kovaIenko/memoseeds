using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace memoseeds.Models.Entities
{

    [Table("categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long CategoryId { get; set; }

        [Column("subject")]
        public long? SubjectId { get; set; }
        public Subject Subject { get; set; }

        [Column("name")]
        public String Name { get; set; }

        //[Column("user")]
        //public long UserId { get; set; }

        public virtual ICollection<Module> Modules { get; set; }

    }
}