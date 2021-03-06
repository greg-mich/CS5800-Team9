using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using backend.Data.Models;
using backend.Data.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace backend.Data.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base (options)
        {
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<StudentEnrollment> StudentEnrollment { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers{ get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentEntityConfiguration());
            modelBuilder.ApplyConfiguration(new InstructorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CourseEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RegistrationEntityConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentEntityConfiguration());
        }

    }
}