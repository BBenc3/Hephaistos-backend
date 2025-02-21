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
        public virtual DbSet<PickedLessons> PickedLessons { get; set; } // Hiányzott a DbSet

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity táblák prefix beállítása (opcionális)
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            // RefreshToken kapcsolat
            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PickedLessons kapcsolatok
            builder.Entity<PickedLessons>()
                .HasOne(pl => pl.Student)
                .WithMany(s => s.StudentLessons)
                .HasForeignKey(pl => pl.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PickedLessons>()
                .HasOne(pl => pl.Lesson)
                .WithMany(l => l.PickedLessons)
                .HasForeignKey(pl => pl.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
