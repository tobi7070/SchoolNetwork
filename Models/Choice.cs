using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class Choice
    {
        public int ChoiceID { get; set; }
        public int ResultID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }

        public Result Result { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }
}
