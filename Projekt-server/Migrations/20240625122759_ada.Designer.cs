﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projekt_server.Data;

#nullable disable

namespace Projekt_server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240625122759_ada")]
    partial class ada
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Projekt_server.Entities.App", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServerId")
                        .HasColumnType("int");

                    b.Property<string>("ServerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("Projekt_server.Entities.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Projekt_server.Entities.Taskk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AppId")
                        .HasColumnType("int");

                    b.Property<string>("AppName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServerId")
                        .HasColumnType("int");

                    b.Property<string>("ServerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("ServerId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Projekt_server.Entities.App", b =>
                {
                    b.HasOne("Projekt_server.Entities.Server", "Server")
                        .WithMany("Apps")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Projekt_server.Entities.Taskk", b =>
                {
                    b.HasOne("Projekt_server.Entities.App", "App")
                        .WithMany("Tasks")
                        .HasForeignKey("AppId");

                    b.HasOne("Projekt_server.Entities.Server", "Server")
                        .WithMany("Tasks")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("App");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("Projekt_server.Entities.App", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Projekt_server.Entities.Server", b =>
                {
                    b.Navigation("Apps");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
