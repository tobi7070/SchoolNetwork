using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models.ViewModels
{
    public class UserResultViewModel
    {
        public UserResultViewModel()
        {
            Results = new List<Result>();
            Assignments = new List<Assignment>();
        }
        public List<Result> Results { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
