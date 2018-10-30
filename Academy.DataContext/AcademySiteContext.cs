using System;
using Academy.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Academy.DataContext
{
    public class AcademySiteContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AcademySiteContext(DbContextOptions<AcademySiteContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<EnrolledStudent> EnrolledStudents { get; set; }

        public DbSet<Grade> Grades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "Administrator", NormalizedName = "ADMINISTRATOR" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Name = "Teacher", NormalizedName = "TEACHER" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 3, Name = "Student", NormalizedName = "STUDENT" });

            modelBuilder.Entity<EnrolledStudent>()
                .HasKey(e => new { e.CourseId, e.StudentId });

            modelBuilder.Entity<EnrolledStudent>()
                .HasOne(es => es.Student)
                .WithMany(s => s.EnrolledStudents)
                .HasForeignKey(es => es.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EnrolledStudent>()
                .HasOne(es => es.Course)
                .WithMany(c => c.EnrolledStudents)
                .HasForeignKey(es => es.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasKey(g => new { g.AssignmentId, g.StudentId });

            modelBuilder.Entity<Grade>()
                .HasOne(gr => gr.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(gr => gr.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasOne(gr => gr.Assignment)
                .WithMany(a => a.Grades)
                .HasForeignKey(g => g.AssignmentId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}

