using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Data
{
    public class InMemoryContext : IdentityDbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options)
            : base(options)
        {

        }
    }
}
