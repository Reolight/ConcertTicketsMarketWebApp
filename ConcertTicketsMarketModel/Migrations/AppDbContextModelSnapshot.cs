﻿// <auto-generated />
using System;
using ConcertTicketsMarketModel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConcertTicketsMarketWebApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Concerts.Concert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PerformerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PerformerId");

                    b.ToTable("Concerts");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Discount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ConcertId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAbsolute")
                        .HasColumnType("bit");

                    b.Property<string>("Promocode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ConcertId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Performers.Performer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BandId");

                    b.ToTable("Performers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Performer");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BookingTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ConcertId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.HasIndex("ConcertId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Performers.Band", b =>
                {
                    b.HasBaseType("ConcertTicketsMarketModel.Model.Performers.Performer");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Band");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Performers.Singer", b =>
                {
                    b.HasBaseType("ConcertTicketsMarketModel.Model.Performers.Performer");

                    b.Property<int>("VoiceType")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Singer");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Concerts.Concert", b =>
                {
                    b.HasOne("ConcertTicketsMarketModel.Model.Performers.Performer", "Performer")
                        .WithMany("Concerts")
                        .HasForeignKey("PerformerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Performer");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Discount", b =>
                {
                    b.HasOne("ConcertTicketsMarketModel.Model.Concerts.Concert", "Concert")
                        .WithMany("Promocodes")
                        .HasForeignKey("ConcertId");

                    b.Navigation("Concert");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Performers.Performer", b =>
                {
                    b.HasOne("ConcertTicketsMarketModel.Model.Performers.Band", null)
                        .WithMany("Performers")
                        .HasForeignKey("BandId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Ticket", b =>
                {
                    b.HasOne("ConcertTicketsMarketModel.Model.Concerts.Concert", "Concert")
                        .WithMany("Tickets")
                        .HasForeignKey("ConcertId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Concert");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Concerts.Concert", b =>
                {
                    b.Navigation("Promocodes");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Performers.Performer", b =>
                {
                    b.Navigation("Concerts");
                });

            modelBuilder.Entity("ConcertTicketsMarketModel.Model.Performers.Band", b =>
                {
                    b.Navigation("Performers");
                });
#pragma warning restore 612, 618
        }
    }
}
