using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using memoseeds.Models.Entities;

namespace memoseeds.Models.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long UserId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [Column("password")]
        [DataType(DataType.Password)]
        public string Password{ get; set; }

        [Column("credits")]
        public int Credits { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Column("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Column("image")]
        [DataType(DataType.Upload)]
        public Byte[] Img { get; set; }

        public virtual ICollection<Collector> Collectors { get; set; }

        public virtual ICollection<AquiredModules> Aquireds { get; set; }

        public virtual ICollection<VisibleModules> Visibles { get; set; }
    }
}
