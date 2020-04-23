using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public string Title { get; set; }
        public bool Value { get; set; }

        public ICollection<Choice> Choices { get; set; }
        public Question Question { get; set; }
    }
}
