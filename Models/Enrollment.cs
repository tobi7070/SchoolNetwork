namespace SchoolNetwork.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int ApplicationUserID { get; set; }

        public Course Course { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
