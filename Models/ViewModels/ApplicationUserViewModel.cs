using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{   
    public class ApplicationUserViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Role")]
        public Role Role { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("First Name")]
        public string FirstMidName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public enum Role
    {
        [Display(Name = "Student")]
        Student,
        [Display(Name = "Instructor")]
        Instructor
    }
}
