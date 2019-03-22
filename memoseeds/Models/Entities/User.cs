using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using memoseeds.Models.Entities;

namespace memoseeds.Models.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int UserId { get; set; }

        [Required]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [Column("password")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [DataType(DataType.Currency)]
        [Column("money")]
        public decimal Money { get; set; }

        [Required]
        [Column("email")]
        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<Collector> Collectors { get; set; }

        public ICollection<AquiredCourses> Aquireds { get; set; }

        public virtual ICollection<VisibleCourses> Visibles { get; set; }
    }
}
