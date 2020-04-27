using System;
using System.Collections.Generic;

namespace SchoolNetwork.Models
{
    public class Result
    {
        public int ResultID { get; set; }
        public int ApplicationUserID { get; set; }
        public int AssignmentID { get; set; }
        public int Score { get; set; }

        public ICollection<Choice> Choices { get; set; }
        public DateTime ResultDate { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Assignment Assignment { get; set; }
    }
}
