using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models.ViewModels
{
    public class ResultViewModel
    {
        public ResultViewModel()
        {
            Questions = new List<Question>();
            Answers = new List<Answer>();
            Choices = new List<Choice>();
        }
        public Result Result { get; set; }
        public Assignment Assignment { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public List<Choice> Choices { get; set; }
    }
}
