using System.Collections.Generic;

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
