﻿// <auto-generated />
using System;
using LudoEngine.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LudoEngine.Migrations
{
    [DbContext(typeof(LudoContext))]
    partial class LudoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LudoEngine.Database.DbBoard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastTimePlayed")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Boards");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsFinished = true,
                            LastTimePlayed = new DateTime(2021, 3, 29, 21, 55, 5, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            IsFinished = true,
                            LastTimePlayed = new DateTime(2021, 1, 15, 21, 55, 5, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            IsFinished = true,
                            LastTimePlayed = new DateTime(2021, 5, 2, 21, 55, 5, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            IsFinished = true,
                            LastTimePlayed = new DateTime(2021, 2, 25, 21, 55, 5, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 5,
                            IsFinished = false,
                            LastTimePlayed = new DateTime(2021, 3, 21, 21, 55, 5, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("LudoEngine.Database.DbBoardState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<bool>("IsInBase")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInSafeZone")
                        .HasColumnType("bit");

                    b.Property<int>("PieceNumber")
                        .HasColumnType("int");

                    b.Property<int>("PiecePosition")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("PlayerId");

                    b.ToTable("BoardStates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BoardId = 5,
                            IsInBase = false,
                            IsInSafeZone = false,
                            PieceNumber = 1,
                            PiecePosition = 18,
                            PlayerId = 1
                        },
                        new
                        {
                            Id = 2,
                            BoardId = 5,
                            IsInBase = false,
                            IsInSafeZone = false,
                            PieceNumber = 2,
                            PiecePosition = 13,
                            PlayerId = 1
                        },
                        new
                        {
                            Id = 3,
                            BoardId = 5,
                            IsInBase = true,
                            IsInSafeZone = false,
                            PieceNumber = 3,
                            PiecePosition = 0,
                            PlayerId = 1
                        },
                        new
                        {
                            Id = 4,
                            BoardId = 5,
                            IsInBase = true,
                            IsInSafeZone = false,
                            PieceNumber = 4,
                            PiecePosition = 0,
                            PlayerId = 1
                        },
                        new
                        {
                            Id = 5,
                            BoardId = 5,
                            IsInBase = false,
                            IsInSafeZone = true,
                            PieceNumber = 1,
                            PiecePosition = 0,
                            PlayerId = 2
                        },
                        new
                        {
                            Id = 6,
                            BoardId = 5,
                            IsInBase = false,
                            IsInSafeZone = false,
                            PieceNumber = 2,
                            PiecePosition = 22,
                            PlayerId = 2
                        },
                        new
                        {
                            Id = 7,
                            BoardId = 5,
                            IsInBase = true,
                            IsInSafeZone = false,
                            PieceNumber = 3,
                            PiecePosition = 0,
                            PlayerId = 2
                        },
                        new
                        {
                            Id = 8,
                            BoardId = 5,
                            IsInBase = true,
                            IsInSafeZone = false,
                            PieceNumber = 4,
                            PiecePosition = 0,
                            PlayerId = 2
                        });
                });

            modelBuilder.Entity("LudoEngine.Database.DbColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ColorCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Colors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ColorCode = ""
                        },
                        new
                        {
                            Id = 2,
                            ColorCode = ""
                        },
                        new
                        {
                            Id = 3,
                            ColorCode = ""
                        },
                        new
                        {
                            Id = 4,
                            ColorCode = ""
                        });
                });

            modelBuilder.Entity("LudoEngine.Database.DbPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ColorId");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ColorId = 2,
                            Name = "Lisa"
                        },
                        new
                        {
                            Id = 2,
                            ColorId = 1,
                            Name = "Bobby"
                        },
                        new
                        {
                            Id = 3,
                            ColorId = 3,
                            Name = "Anna"
                        },
                        new
                        {
                            Id = 4,
                            ColorId = 4,
                            Name = "Luke"
                        },
                        new
                        {
                            Id = 5,
                            ColorId = 1,
                            Name = "Roseline"
                        },
                        new
                        {
                            Id = 6,
                            ColorId = 2,
                            Name = "Noa"
                        });
                });

            modelBuilder.Entity("LudoEngine.Database.DbWinner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<int>("Placement")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Winners");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BoardId = 1,
                            Placement = 1,
                            PlayerId = 2
                        },
                        new
                        {
                            Id = 2,
                            BoardId = 1,
                            Placement = 2,
                            PlayerId = 1
                        },
                        new
                        {
                            Id = 3,
                            BoardId = 2,
                            Placement = 2,
                            PlayerId = 3
                        },
                        new
                        {
                            Id = 4,
                            BoardId = 2,
                            Placement = 3,
                            PlayerId = 4
                        },
                        new
                        {
                            Id = 5,
                            BoardId = 2,
                            Placement = 4,
                            PlayerId = 6
                        },
                        new
                        {
                            Id = 6,
                            BoardId = 2,
                            Placement = 1,
                            PlayerId = 5
                        });
                });

            modelBuilder.Entity("LudoEngine.Database.DbBoardState", b =>
                {
                    b.HasOne("LudoEngine.Database.DbBoard", "Board")
                        .WithMany()
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LudoEngine.Database.DbPlayer", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("LudoEngine.Database.DbPlayer", b =>
                {
                    b.HasOne("LudoEngine.Database.DbColor", "Color")
                        .WithMany()
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Color");
                });

            modelBuilder.Entity("LudoEngine.Database.DbWinner", b =>
                {
                    b.HasOne("LudoEngine.Database.DbBoard", "Board")
                        .WithMany()
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LudoEngine.Database.DbPlayer", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
