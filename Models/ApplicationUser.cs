using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolNetwork.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstMidName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        public DateTime JoinDate { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Result> Results { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
