using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolNetwork.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        [Display(Name = "Course")]
        public string Title { get; set; }
        public int Credits { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
