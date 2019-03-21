using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("aquired_courses")]
    public class AquiredCourses
    {
        [Key]
        [Column("id")]
        public int AquiredCoursesId { get; set; }

        [Column("user")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        [Column("is_local")]
        public Boolean IsLocal { get; set; }
    }
}
