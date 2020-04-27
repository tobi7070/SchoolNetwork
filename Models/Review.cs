namespace SchoolNetwork.Models
{
    public enum Score
    {
        A, B, C, D, F, E
    }

    public class Review
    {
        public int ReviewID { get; set; }
        public int RatingID { get; set; }
        public int ApplicationUserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Score? Score { get; set; }

        public Rating Rating { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
