using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SchoolNetwork.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
