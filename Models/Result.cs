using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class Result
    {
        public int ResultID { get; set; }
        public int AssignmentID { get; set; }
        public int Score { get; set; }

        public ICollection<Choice> Choices { get; set; }
        public DateTime ResultDate { get; set; }
        public Assignment Assignment { get; set; }
    }
}
