﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolNetwork.Models;
using System;
using System.Linq;

namespace SchoolNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            // Soft-Delete 
            var entities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Metadata.GetProperties()
                .Any(x => x.Name == "IsDeleted"))
                .ToList();

            foreach (var entity in entities)
            {
                entity.State = EntityState.Unchanged;
                entity.CurrentValues["IsDeleted"] = true;
            }

            return base.SaveChanges();
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Assignment>().ToTable("Assignment");
            modelBuilder.Entity<Question>().ToTable("Question").Property<bool>("isDeleted");
            modelBuilder.Entity<Answer>().ToTable("Answer").Property<bool>("isDeleted");
            modelBuilder.Entity<Choice>().ToTable("Choice");
            modelBuilder.Entity<Result>().ToTable("Result");
            modelBuilder.Entity<Review>().ToTable("Review");

            modelBuilder.Entity<Assignment>().HasMany(a => a.Questions).WithOne(b => b.Assignment).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Assignment>().HasMany(a => a.Reviews).WithOne(b => b.Assignment).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Question>().HasMany(a => a.Answers).WithOne(b => b.Question).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Answer>().HasOne(a => a.Question).WithMany(b => b.Answers).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Choice>().HasOne(a => a.Result).WithMany(b => b.Choices).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Choice>().HasOne(a => a.Question).WithMany(b => b.Choices).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Choice>().HasOne(a => a.Answer).WithMany(b => b.Choices).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Result>().HasMany(a => a.Choices).WithOne(b => b.Result).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Result>().HasOne(a => a.ApplicationUser).WithMany(b => b.Results).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>().HasOne(a => a.Assignment).WithMany(b => b.Reviews).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>().HasOne(a => a.ApplicationUser).WithMany(b => b.Reviews).OnDelete(DeleteBehavior.Restrict);

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

            ApplicationUser User1 = new ApplicationUser { Id = 1, UserName = "JohnDoe", EmailConfirmed = true, FirstMidName = "John", LastName = "Doe", SecurityStamp = Guid.NewGuid().ToString("D") };
            User1.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(User1, "qwerty");
            ApplicationUser User2 = new ApplicationUser { Id = 2, UserName = "JaneDoe", EmailConfirmed = true, FirstMidName = "Jane", LastName = "Doe", SecurityStamp = Guid.NewGuid().ToString("D") };
            User2.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(User2, "iloveyou");
            modelBuilder.Entity<ApplicationUser>().HasData(User1, User2);

            modelBuilder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = 1, Name = "Student", NormalizedName = "STUDENT", Description = "A role for students" },
                new ApplicationRole { Id = 2, Name = "Instructor", NormalizedName = "INSTRUCTOR", Description = "A role for instructors" }
            );

            modelBuilder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole { RoleId = 1, UserId = 2 },
                new ApplicationUserRole { RoleId = 2, UserId = 1 }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseID = 1,
                    Title = "Mathmatics",
                    Credits = 10
                },
                new Course
                {
                    CourseID = 2,
                    Title = "Physics",
                    Credits = 8
                }
            );

            modelBuilder.Entity<Assignment>().HasData(
                new Assignment
                {
                    AssignmentID = 1,
                    ApplicationUserID = 1,
                    CourseID = 1,
                    Title = "Linear Algebra",
                    Value = 15
                },
                new Assignment
                {
                    AssignmentID = 2,
                    ApplicationUserID = 1,
                    CourseID = 2,
                    Title = "Magnetic Force",
                    Value = 10
                }
            );

            modelBuilder.Entity<Question>().HasData(
                new Question
                {
                    QuestionID = 1,
                    AssignmentID = 1,
                    Title = "Something something",
                    Value = 5,
                    isDeleted = false
                },
                new Question
                {
                    QuestionID = 2,
                    AssignmentID = 1,
                    Title = "Something something",
                    Value = 5,
                    isDeleted = false
                },
                new Question
                {
                    QuestionID = 3,
                    AssignmentID = 1,
                    Title = "Something something",
                    Value = 5,
                    isDeleted = false
                },
                new Question
                {
                    QuestionID = 4,
                    AssignmentID = 2,
                    Title = "Something something",
                    Value = 5,
                    isDeleted = false
                },
                new Question
                {
                    QuestionID = 5,
                    AssignmentID = 2,
                    Title = "Something something",
                    Value = 5,
                    isDeleted = false
                }
            ); ;

            modelBuilder.Entity<Answer>().HasData(
                new Answer
                {
                    AnswerID = 1,
                    QuestionID = 1,
                    Title = "Something something",
                    Value = true,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 2,
                    QuestionID = 1,
                    Title = "Something something",
                    Value = false,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 3,
                    QuestionID = 2,
                    Title = "Something something",
                    Value = false,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 4,
                    QuestionID = 2,
                    Title = "Something something",
                    Value = true,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 5,
                    QuestionID = 3,
                    Title = "Something something",
                    Value = true,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 6,
                    QuestionID = 3,
                    Title = "Something something",
                    Value = false,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 7,
                    QuestionID = 4,
                    Title = "Something something",
                    Value = false,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 8,
                    QuestionID = 4,
                    Title = "Something something",
                    Value = true,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 9,
                    QuestionID = 5,
                    Title = "Something something",
                    Value = true,
                    isDeleted = false
                },
                new Answer
                {
                    AnswerID = 10,
                    QuestionID = 5,
                    Title = "Something something",
                    Value = false,
                    isDeleted = false
                }
            );
        }
    }
}
