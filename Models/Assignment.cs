using System.Collections.Generic;
using System.ComponentModel;

namespace SchoolNetwork.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int CourseID { get; set; }
        public int ApplicationUserID { get; set; }
        [DisplayName("Assignment")]
        public string Title { get; set; }
        public int Value { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Course Course { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
