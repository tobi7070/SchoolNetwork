using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstMidName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
