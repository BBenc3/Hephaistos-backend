using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectHephaistos.Models;

namespace ProjectHephaistos.Data;

public partial class HephaistosContext : DbContext
{
    public HephaistosContext()
    {
    }

    public HephaistosContext(DbContextOptions<HephaistosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auditlog> Auditlogs { get; set; }

    public virtual DbSet<SubjectSchedule> Subjectschedules { get; set; }

    public virtual DbSet<Completedsubject> Completedsubjects { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<University> Universities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<GeneratedTimetable> GeneratedTimetables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("Relational:Collation", "utf8mb4_general_ci");
        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Auditlog");

            entity.ToTable("auditlog");

            entity.HasIndex(e => e.ChangedBy, "ChangedBy");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.ChangedBy).HasColumnType("int");
            entity.Property(e => e.ChangedData).HasColumnType("json");
            entity.Property(e => e.OperationType).HasMaxLength(50); // Changed from enum to string
            entity.Property(e => e.RecordId).HasColumnType("int");
            entity.Property(e => e.TableName).HasMaxLength(255);

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.Auditlogs)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("auditlog_ibfk_1");
        });

        modelBuilder.Entity<SubjectSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Classschedule");

            entity.ToTable("classschedules"); // Updated table name

            entity.HasIndex(e => e.SubjectId, "SubjectId");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.DayOfWeek).HasMaxLength(20);
            entity.Property(e => e.EndTime).HasColumnType("time");
            entity.Property(e => e.StartTime).HasColumnType("time");
            entity.Property(e => e.SubjectId).HasColumnType("int");
            entity.Property(e => e.Year).HasColumnType("int");

            entity.HasOne(d => d.Subject).WithMany(p => p.Subjectschedules)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Subjectschedules_ibfk_1");
        });

        modelBuilder.Entity<Completedsubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Completedsubject");

            entity.ToTable("completedsubjects");

            entity.HasIndex(e => e.SubjectId, "SubjectId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.SubjectId).HasColumnType("int");
            entity.Property(e => e.UserId).HasColumnType("int");

            entity.HasOne(d => d.Subject).WithMany(p => p.Completedsubjects)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("completedsubjects_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Completedsubjects)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("completedsubjects_ibfk_1");
        });


        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Major");

            entity.ToTable("majors");

            entity.HasIndex(e => e.UniversityId, "UniversityId");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.UniversityId).HasColumnType("int");

            entity.HasOne(d => d.University).WithMany(p => p.Majors)
                .HasForeignKey(d => d.UniversityId)
                .HasConstraintName("majors_ibfk_1");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Refreshtoken");

            entity.ToTable("refreshtokens");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.Created).HasMaxLength(6);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Expires).HasMaxLength(6);
            entity.Property(e => e.Revoked).HasMaxLength(6);
            entity.Property(e => e.UserId).HasColumnType("int");

            entity.HasOne(d => d.User).WithMany(p => p.Refreshtokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("refreshtokens_ibfk_1");
        });

        modelBuilder.Entity<Subject>(entity =>
  {
      entity.HasKey(e => e.Id).HasName("PK_Subject");

      entity.ToTable("subjects");

      entity.HasIndex(e => e.MajorId, "MajorId");

      entity.Property(e => e.Id).HasColumnType("int");
      entity.Property(e => e.Active).HasDefaultValueSql("'1'");
      entity.Property(e => e.Code).HasMaxLength(255);
      entity.Property(e => e.CreatedAt)
          .HasDefaultValueSql("current_timestamp()")
          .HasColumnType("timestamp");
      entity.Property(e => e.CreditValue).HasColumnType("int");
      entity.Property(e => e.IsElective).HasDefaultValueSql("'0'");
      entity.Property(e => e.IsEvenSemester).HasDefaultValueSql("'0'");
      entity.Property(e => e.MajorId).HasColumnType("int");
      entity.Property(e => e.Name).HasMaxLength(255);
      entity.Property(e => e.Note).HasColumnType("text");

      entity.HasOne(d => d.Major).WithMany(p => p.Subjects)
          .HasForeignKey(d => d.MajorId)
          .HasConstraintName("subjects_ibfk_1");

      // Előfeltételek Many-to-Many kapcsolat
      entity.HasMany(s => s.PrerequisiteSubjects)
          .WithMany(s => s.RequiredForSubjects)
          .UsingEntity<Dictionary<string, object>>(
              "SubjectPrerequisites",
              j => j.HasOne<Subject>().WithMany().HasForeignKey("PrerequisiteId"),
              j => j.HasOne<Subject>().WithMany().HasForeignKey("SubjectId")
          );
  });

        modelBuilder.Entity<GeneratedTimetable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GeneratedTimetable");

            entity.ToTable("generatedtimetables");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnType("int");

            entity.HasOne(d => d.User).WithMany(p => p.GeneratedTimetables)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("generatedtimetables_ibfk_1");
        });

        modelBuilder.Entity<University>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_University");

            entity.ToTable("universities");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.Place).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User");

            entity.ToTable("users");

            entity.HasIndex(e => e.MajorId, "fk_users_major");

            entity.Property(e => e.Id).HasColumnType("int");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.MajorId).HasColumnType("int");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.ProfilePicturepath).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
            entity.Property(e => e.StartYear).HasColumnType("int");
            entity.Property(e => e.Status).HasMaxLength(50); // Changed from enum to string
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.Major).WithMany(p => p.Users)
                .HasForeignKey(d => d.MajorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_major");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
