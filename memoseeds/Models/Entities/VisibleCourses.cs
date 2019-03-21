using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace memoseeds.Models.Entities
{
    [Table("visible_courses")]
    public class VisibleCourses
    {
        [Key]
        [Column("id")]
        public int VisibleCoursesId { get; set; }

        [Column("user")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Column("course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Column("can_edit")]
        public Boolean CanEdit { get; set; }
    }
}
