using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class Grade
    {
        public int GradeID { get; set; }
        public int ApplicationUserID { get; set; }
        public int ResultID { get; set; }

        public ICollection<Result> Results { get; set; }
        public ApplicationUser ApplicationUser;
        public Result Result;
    }
}
