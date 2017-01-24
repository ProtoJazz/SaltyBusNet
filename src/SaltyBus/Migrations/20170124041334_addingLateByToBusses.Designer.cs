using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SaltyBus.Models;

namespace SaltyBus.Migrations
{
    [DbContext(typeof(SaltyContext))]
    [Migration("20170124041334_addingLateByToBusses")]
    partial class addingLateByToBusses
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SaltyBus.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Url");

                    b.HasKey("BlogId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("SaltyBus.Models.Bus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Arrived");

                    b.Property<DateTime>("Estimated");

                    b.Property<string>("Key");

                    b.Property<int>("LateBy");

                    b.Property<int>("RouteNumber");

                    b.Property<DateTime>("Scheduled");

                    b.HasKey("Id");

                    b.ToTable("Buses");
                });

            modelBuilder.Entity("SaltyBus.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlogId");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("PostId");

                    b.HasIndex("BlogId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("SaltyBus.Models.Post", b =>
                {
                    b.HasOne("SaltyBus.Models.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
