﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
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

    public virtual DbSet<Classschedule> Classschedules { get; set; }

    public virtual DbSet<Completedsubject> Completedsubjects { get; set; }

    public virtual DbSet<Course> Courses { get; set; }


    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<University> Universities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=hephaistos;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Auditlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("auditlog");

            entity.HasIndex(e => e.ChangedBy, "ChangedBy");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.ChangedBy).HasColumnType("int(11)");
            entity.Property(e => e.ChangedData).HasColumnType("json");
            entity.Property(e => e.OperationType).HasColumnType("enum('INSERT','UPDATE','DELETE')");
            entity.Property(e => e.RecordId).HasColumnType("int(11)");
            entity.Property(e => e.TableName).HasMaxLength(255);

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.Auditlogs)
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("auditlog_ibfk_1");
        });

        modelBuilder.Entity<Classschedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("classschedules");

            entity.HasIndex(e => e.SubjectId, "SubjectId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.DayOfWeek).HasMaxLength(20);
            entity.Property(e => e.EndTime).HasColumnType("time");
            entity.Property(e => e.StartTime).HasColumnType("time");
            entity.Property(e => e.SubjectId).HasColumnType("int(11)");
            entity.Property(e => e.Year).HasColumnType("int(11)");

            entity.HasOne(d => d.Subject).WithMany(p => p.Classschedules)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("classschedules_ibfk_1");
        });

        modelBuilder.Entity<Completedsubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("completedsubjects");

            entity.HasIndex(e => e.SubjectId, "SubjectId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.SubjectId).HasColumnType("int(11)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Subject).WithMany(p => p.Completedsubjects)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("completedsubjects_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Completedsubjects)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("completedsubjects_ibfk_1");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("course");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt).HasMaxLength(6);
        });


        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("majors");

            entity.HasIndex(e => e.UniversityId, "UniversityId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.UniversityId).HasColumnType("int(11)");

            entity.HasOne(d => d.University).WithMany(p => p.Majors)
                .HasForeignKey(d => d.UniversityId)
                .HasConstraintName("majors_ibfk_1");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("refreshtokens");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.Created).HasMaxLength(6);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Expires).HasMaxLength(6);
            entity.Property(e => e.Revoked).HasMaxLength(6);
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Refreshtokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("refreshtokens_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subjects");

            entity.HasIndex(e => e.MajorId, "MajorId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.CreditValue).HasColumnType("int(11)");
            entity.Property(e => e.IsElective).HasDefaultValueSql("'0'");
            entity.Property(e => e.IsEvenSemester).HasDefaultValueSql("'0'");
            entity.Property(e => e.MajorId).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Note).HasColumnType("text");

            entity.HasOne(d => d.Major).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.MajorId)
                .HasConstraintName("subjects_ibfk_1");
        });

        modelBuilder.Entity<University>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("universities");

            entity.Property(e => e.Id).HasColumnType("int(11)");
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
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.MajorId, "fk_users_major");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Active).HasDefaultValueSql("'1'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.MajorId).HasColumnType("int(11)");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.ProfilePicturepath).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
            entity.Property(e => e.StartYear).HasColumnType("int(11)");
            entity.Property(e => e.Status).HasColumnType("enum('active','passive')");
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
