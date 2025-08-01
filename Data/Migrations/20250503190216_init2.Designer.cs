﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManagement.Data.Context;

#nullable disable

namespace TaskManagement.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250503190216_init2")]
    partial class init2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskManagement.Data.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("RefreshTokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedFromIp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RefreshTokenId");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.TaskDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangeTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("FieldName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<int>("UpdatedById")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("TaskDetails");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ChangeTime = new DateTime(2025, 5, 4, 0, 32, 15, 413, DateTimeKind.Local).AddTicks(1268),
                            FieldName = "Status",
                            NewValue = "Todo",
                            TaskId = 1,
                            UpdatedById = 1
                        });
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.TaskEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AssigneeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AssigneeId = 1,
                            CreatedAt = new DateTime(2025, 5, 4, 0, 32, 15, 412, DateTimeKind.Local).AddTicks(7414),
                            CreatorId = 1,
                            Description = "This is a seeded task for demo purposes.",
                            DueDate = new DateTime(2025, 5, 11, 0, 32, 15, 412, DateTimeKind.Local).AddTicks(6467),
                            IsDeleted = false,
                            Priority = 1,
                            Status = 0,
                            Title = "Initial Task",
                            UpdatedAt = new DateTime(2025, 5, 4, 0, 32, 15, 412, DateTimeKind.Local).AddTicks(7923)
                        });
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 5, 4, 0, 32, 15, 165, DateTimeKind.Local).AddTicks(6126),
                            Email = "admin@gmail.com",
                            IsActive = true,
                            PasswordHash = "$2a$11$A/WAuusYrNH.44VRkkiGQuRS7KBm1rIZz/E94ZB2VDCEpQokh2evy",
                            Role = 0
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 5, 4, 0, 32, 15, 411, DateTimeKind.Local).AddTicks(3364),
                            Email = "tanmay@gmail.com",
                            IsActive = true,
                            PasswordHash = "$2a$11$QY19Ergj5AX/yA3EGw9WJOQ2pxw0436jeOPa9j4KVKKB8nEhPrqNW",
                            Role = 1
                        });
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.RefreshToken", b =>
                {
                    b.HasOne("TaskManagement.Data.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.TaskDetail", b =>
                {
                    b.HasOne("TaskManagement.Data.Entities.TaskEntity", "Task")
                        .WithMany("TaskDetails")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskManagement.Data.Entities.User", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");

                    b.Navigation("UpdatedBy");
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.TaskEntity", b =>
                {
                    b.HasOne("TaskManagement.Data.Entities.User", "Assignee")
                        .WithMany("AssignedTasks")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TaskManagement.Data.Entities.User", "Creator")
                        .WithMany("CreatedTasks")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.TaskEntity", b =>
                {
                    b.Navigation("TaskDetails");
                });

            modelBuilder.Entity("TaskManagement.Data.Entities.User", b =>
                {
                    b.Navigation("AssignedTasks");

                    b.Navigation("CreatedTasks");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
