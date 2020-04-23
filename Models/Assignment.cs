using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int CourseID { get; set; }
        public int ApplicationUserID { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }

        public ICollection<Question> Questions { get; set; }
        public Course Course { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
