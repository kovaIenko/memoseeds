using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    public class User
    {

        public User(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "id")]
        public long userId { get; set; }

        public string name { get; set; }

        public string password{ get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal money { get; set; }

        public string email { get; set; }

        public virtual ICollection<Collector> collectors { get; set; }
    }
}
