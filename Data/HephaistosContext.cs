using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.Data;

public partial class HephaistosContext : DbContext
{
    public HephaistosContext(DbContextOptions<HephaistosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Teacher> Teachers { get; set; }
    public virtual DbSet<Userdata> Userdatas { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }
    public virtual DbSet<Student> Students { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PickedLessons kapcsolatok
        modelBuilder.Entity<PickedLessons>()
            .HasOne(pl => pl.Student)
            .WithMany(s => s.StudentLessons)
            .HasForeignKey(pl => pl.StudentId);

        modelBuilder.Entity<PickedLessons>()
            .HasOne(pl => pl.Lesson)
            .WithMany(l => l.PickedLessons)
            .HasForeignKey(pl => pl.LessonId);
    }

}

