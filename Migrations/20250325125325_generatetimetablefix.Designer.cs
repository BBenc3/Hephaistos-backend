﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHephaistos.Data;

#nullable disable

namespace ProjectHephaistos.Migrations
{
    [DbContext(typeof(HephaistosContext))]
    [Migration("20250325125325_generatetimetablefix")]
    partial class generatetimetablefix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectHephaistos.Models.Auditlog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("ChangedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int?>("ChangedBy")
                        .HasColumnType("int");

                    b.Property<string>("ChangedData")
                        .HasColumnType("json");

                    b.Property<string>("OperationType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("RecordId")
                        .HasColumnType("int");

                    b.Property<string>("TableName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK_Auditlog");

                    b.HasIndex(new[] { "ChangedBy" }, "ChangedBy");

                    b.ToTable("auditlog", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Completedsubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("active")
                        .HasDefaultValueSql("'1'");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Completedsubject");

                    b.HasIndex(new[] { "SubjectId" }, "SubjectId");

                    b.HasIndex(new[] { "UserId" }, "UserId");

                    b.ToTable("completedsubjects", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.GeneratedTimetable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'1'");

                    b.Property<byte[]>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_GeneratedTimetable");

                    b.HasIndex(new[] { "UserId" }, "UserId");

                    b.ToTable("generatedtimetables", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Major", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'1'");

                    b.Property<byte[]>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<int?>("UniversityId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Major");

                    b.HasIndex(new[] { "UniversityId" }, "UniversityId");

                    b.ToTable("majors", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Refreshtoken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'1'");

                    b.Property<DateTime?>("Created")
                        .HasMaxLength(6)
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<DateTime?>("Expires")
                        .HasMaxLength(6)
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Revoked")
                        .HasMaxLength(6)
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Refreshtoken");

                    b.HasIndex(new[] { "UserId" }, "UserId");

                    b.ToTable("refreshtokens", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'1'");

                    b.Property<string>("Code")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<int?>("CreditValue")
                        .HasColumnType("int");

                    b.Property<int?>("GeneratedTimetableId")
                        .HasColumnType("int");

                    b.Property<bool>("IsElective")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'0'");

                    b.Property<bool>("IsEvenSemester")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'0'");

                    b.Property<int?>("MajorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("PK_Subject");

                    b.HasIndex("GeneratedTimetableId");

                    b.HasIndex(new[] { "MajorId" }, "MajorId");

                    b.ToTable("subjects", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.SubjectSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasColumnName("active")
                        .HasDefaultValueSql("'1'");

                    b.Property<byte[]>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<string>("DayOfWeek")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<TimeOnly?>("EndTime")
                        .HasColumnType("time");

                    b.Property<int?>("GeneratedTimetableId")
                        .HasColumnType("int");

                    b.Property<TimeOnly?>("StartTime")
                        .HasColumnType("time");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK_Classschedule");

                    b.HasIndex("GeneratedTimetableId");

                    b.HasIndex(new[] { "SubjectId" }, "SubjectId");

                    b.ToTable("classschedules", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.University", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'1'");

                    b.Property<byte[]>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("Place")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK_University");

                    b.ToTable("universities", (string)null);
                });

            modelBuilder.Entity("ProjectHephaistos.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("'1'");

                    b.Property<byte[]>("CreatedAt")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("current_timestamp()");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("MajorId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ProfilePicturepath")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Role")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("StartYear")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Username")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id")
                        .HasName("PK_User");

                    b.HasIndex(new[] { "MajorId" }, "fk_users_major");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("SubjectPrerequisites", b =>
                {
                    b.Property<int>("PrerequisiteId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("PrerequisiteId", "SubjectId");

                    b.HasIndex("SubjectId");

                    b.ToTable("SubjectPrerequisites");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Auditlog", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.User", "ChangedByNavigation")
                        .WithMany("Auditlogs")
                        .HasForeignKey("ChangedBy")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("auditlog_ibfk_1");

                    b.Navigation("ChangedByNavigation");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Completedsubject", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.Subject", "Subject")
                        .WithMany("Completedsubjects")
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("completedsubjects_ibfk_2");

                    b.HasOne("ProjectHephaistos.Models.User", "User")
                        .WithMany("Completedsubjects")
                        .HasForeignKey("UserId")
                        .HasConstraintName("completedsubjects_ibfk_1");

                    b.Navigation("Subject");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.GeneratedTimetable", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.User", "User")
                        .WithMany("GeneratedTimetables")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("generatedtimetables_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Major", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.University", "University")
                        .WithMany("Majors")
                        .HasForeignKey("UniversityId")
                        .HasConstraintName("majors_ibfk_1");

                    b.Navigation("University");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Refreshtoken", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.User", "User")
                        .WithMany("Refreshtokens")
                        .HasForeignKey("UserId")
                        .HasConstraintName("refreshtokens_ibfk_1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Subject", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.GeneratedTimetable", null)
                        .WithMany("OmittedSubjects")
                        .HasForeignKey("GeneratedTimetableId");

                    b.HasOne("ProjectHephaistos.Models.Major", "Major")
                        .WithMany("Subjects")
                        .HasForeignKey("MajorId")
                        .HasConstraintName("subjects_ibfk_1");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.SubjectSchedule", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.GeneratedTimetable", null)
                        .WithMany("ClassSchedules")
                        .HasForeignKey("GeneratedTimetableId");

                    b.HasOne("ProjectHephaistos.Models.Subject", "Subject")
                        .WithMany("Subjectschedules")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("Subjectschedules_ibfk_1");

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.User", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.Major", "Major")
                        .WithMany("Users")
                        .HasForeignKey("MajorId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_users_major");

                    b.Navigation("Major");
                });

            modelBuilder.Entity("SubjectPrerequisites", b =>
                {
                    b.HasOne("ProjectHephaistos.Models.Subject", null)
                        .WithMany()
                        .HasForeignKey("PrerequisiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectHephaistos.Models.Subject", null)
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectHephaistos.Models.GeneratedTimetable", b =>
                {
                    b.Navigation("ClassSchedules");

                    b.Navigation("OmittedSubjects");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Major", b =>
                {
                    b.Navigation("Subjects");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.Subject", b =>
                {
                    b.Navigation("Completedsubjects");

                    b.Navigation("Subjectschedules");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.University", b =>
                {
                    b.Navigation("Majors");
                });

            modelBuilder.Entity("ProjectHephaistos.Models.User", b =>
                {
                    b.Navigation("Auditlogs");

                    b.Navigation("Completedsubjects");

                    b.Navigation("GeneratedTimetables");

                    b.Navigation("Refreshtokens");
                });
#pragma warning restore 612, 618
        }
    }
}
