using System.Collections.Generic;
using System.ComponentModel;

namespace SchoolNetwork.Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public int AssignmentID { get; set; }
        [DisplayName("Question")]
        public string Title { get; set; }
        public int Value { get; set; }
        public bool isDeleted { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public ICollection<Choice> Choices { get; set; }
        public Assignment Assignment { get; set; }
    }
}
