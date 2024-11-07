﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentPortal.Data;

#nullable disable

namespace StudentPortal.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241107061652_TestTry")]
    partial class TestTry
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentDetail", b =>
                {
                    b.Property<int>("StudId")
                        .HasColumnType("int");

                    b.Property<string>("EDPCode")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("SubjectCode")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("StudId", "EDPCode");

                    b.HasIndex("EDPCode");

                    b.ToTable("EnrollmentDetails");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Encoder")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<DateTime>("EnrollDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SchoolYear")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("TotalUnits")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("EnrollmentHeaders");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Subject", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Curriculum")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<int>("Offering")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<int>("Units")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectPreq", b =>
                {
                    b.Property<string>("SubjectCode")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("PreCode")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("SubjectCode", "PreCode");

                    b.HasIndex("SubjectCode")
                        .IsUnique();

                    b.ToTable("Prerequisites");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectSchedule", b =>
                {
                    b.Property<string>("EDPCode")
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("ClassSize")
                        .HasColumnType("int");

                    b.Property<string>("Days")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxSize")
                        .HasColumnType("int");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Section")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("SubjectCode")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("XM")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("EDPCode");

                    b.HasIndex("SubjectCode");

                    b.ToTable("SubjectSchedules");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentDetail", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.SubjectSchedule", "SubjectSchedule")
                        .WithMany("EnrollmentDetails")
                        .HasForeignKey("EDPCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentPortal.Models.Entities.EnrollmentHeader", "EnrollmentHeader")
                        .WithMany("EnrollmentDetails")
                        .HasForeignKey("StudId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EnrollmentHeader");

                    b.Navigation("SubjectSchedule");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentHeader", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectPreq", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.Subject", "Subject")
                        .WithOne("Prerequisites")
                        .HasForeignKey("StudentPortal.Models.Entities.SubjectPreq", "SubjectCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectSchedule", b =>
                {
                    b.HasOne("StudentPortal.Models.Entities.Subject", "Subject")
                        .WithMany("Schedules")
                        .HasForeignKey("SubjectCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.EnrollmentHeader", b =>
                {
                    b.Navigation("EnrollmentDetails");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.Subject", b =>
                {
                    b.Navigation("Prerequisites")
                        .IsRequired();

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("StudentPortal.Models.Entities.SubjectSchedule", b =>
                {
                    b.Navigation("EnrollmentDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
