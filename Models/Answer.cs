using System.Collections.Generic;
using System.ComponentModel;

namespace SchoolNetwork.Models
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        [DisplayName("Answer")]
        public string Title { get; set; }
        [DisplayName("Correct?")]
        public bool Value { get; set; }
        public bool isDeleted { get; set; }

        public ICollection<Choice> Choices { get; set; }
        public Question Question { get; set; }
    }
}
