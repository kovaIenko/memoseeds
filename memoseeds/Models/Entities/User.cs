using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "id")]
        public long UserId { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Money { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Collector> Collectors { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
