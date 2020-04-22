using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            ApplicationUser User1 = new ApplicationUser { Id = 1, UserName = "JohnDoe", EmailConfirmed = true, FirstMidName = "John", LastName = "Doe" };
            User1.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(User1, "qwerty");
            ApplicationUser User2 = new ApplicationUser { Id = 2, UserName = "JaneDoe", EmailConfirmed = true, FirstMidName = "Jane", LastName = "Doe" };
            User1.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(User1, "password");
            modelBuilder.Entity<ApplicationUser>().HasData(User1, User2);

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = 1, Name = "Student", NormalizedName = "STUDENT", Description = "A role for students" },
                new ApplicationRole { Id = 2, Name = "Instructor", NormalizedName = "INSTRUCTOR", Description = "A role for instructors" }
            );

            modelBuilder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole { RoleId = 1, UserId = 2 },
                new ApplicationUserRole { RoleId = 2, UserId = 1 }
            );

        }
    }
}
