using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models.ViewModels
{
    public class ChoiceViewModel
    {
        public int AnswerID { get; set; }
        public bool IsSelected { get; set; }
    }

    public class AssignmentViewModel
    {

        public AssignmentViewModel()
        {
            Choices = new List<ChoiceViewModel>();
            Questions = new List<Question>();
        }
        public Assignment Assignment { get; set; }
        public List<Question> Questions { get; set; }
        public List<ChoiceViewModel> Choices { get; set; }
    }
}
