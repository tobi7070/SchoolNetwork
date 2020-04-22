using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
