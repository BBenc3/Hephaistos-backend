using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.Data
{
    public class HephaistosContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public HephaistosContext(DbContextOptions<HephaistosContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Userdata> Userdatas { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Ensure this line is present

            // Define foreign key relationship for RefreshTokens
            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PickedLessons kapcsolatok
            builder.Entity<PickedLessons>()
                .HasOne(pl => pl.Student)
                .WithMany(s => s.StudentLessons)
                .HasForeignKey(pl => pl.StudentId);

            builder.Entity<PickedLessons>()
                .HasOne(pl => pl.Lesson)
                .WithMany(l => l.PickedLessons)
                .HasForeignKey(pl => pl.LessonId);
        }
    }
}

