using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class Rating
    {
        public int RatingID { get; set; }
        public int AssignmentID { get; set; }
        public int Score { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public Assignment Assignment { get; set; }
    }
}
