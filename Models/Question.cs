using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public int AssignmentID { get; set; }
        public string Title { get; set; }
        public int Value { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public ICollection<Choice> Choices { get; set; }
        public Assignment Assignment { get; set; }
    }
}
