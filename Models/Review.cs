using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolNetwork.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int AssignmentID { get; set; }
        public int ApplicationUserID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Score")]
        public Score Score { get; set; }

        public Assignment Assignment { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
    public enum Score
    {
        [Display(Name = "Very Bad")]
        VeryBad = 1,
        [Display(Name = "Bad")]
        Bad = 2,
        [Display(Name = "Average")]
        Average = 3,
        [Display(Name = "Good")]
        Good = 4,
        [Display(Name = "Very Good")]
        VeryGood = 5
    }
}
