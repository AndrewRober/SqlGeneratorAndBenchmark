﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SqlGeneratorAndBenchmark;

namespace SqlGeneratorAndBenchmark.Migrations
{
    [DbContext(typeof(BenchmarkContext))]
    partial class BenchmarkContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SqlGeneratorAndBenchmark.ChildObj", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChildObjName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentObjId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentObjId");

                    b.ToTable("ChildObjs");
                });

            modelBuilder.Entity("SqlGeneratorAndBenchmark.DeNormalizedObj", b =>
                {
                    b.Property<int>("DeNormalizedObjId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChildObjName1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChildObjName2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChildObjName3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentObjName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeNormalizedObjId");

                    b.ToTable("DeNormalizedObjs");
                });

            modelBuilder.Entity("SqlGeneratorAndBenchmark.ParentObj", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ParentObjName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParentObjs");
                });

            modelBuilder.Entity("SqlGeneratorAndBenchmark.ChildObj", b =>
                {
                    b.HasOne("SqlGeneratorAndBenchmark.ParentObj", "ParentObj")
                        .WithMany("ChildObjs")
                        .HasForeignKey("ParentObjId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentObj");
                });

            modelBuilder.Entity("SqlGeneratorAndBenchmark.ParentObj", b =>
                {
                    b.Navigation("ChildObjs");
                });
#pragma warning restore 612, 618
        }
    }
}