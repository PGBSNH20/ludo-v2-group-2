using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Database
{
    public class LudoContext : DbContext
    {
        public DbSet<DbBoard> Boards { get; set; }
        public DbSet<DbBoardState> BoardStates { get; set; }
        public DbSet<DbColor> Colors { get; set; }
        public DbSet<DbPlayer> Players { get; set; }
        public DbSet<DbWinner> Winners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // production: optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-AFKC3I2\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Initial Catalog=LudoGame");
            // testing: optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-AFKC3I2\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Initial Catalog=LudoGameTesting");
            //optionsBuilder.UseSqlServer(@"Server = DESKTOP-7NBHFKN; Database = LudoGame; Trusted_Connection = True;");
            //optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-AFKC3I2\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Initial Catalog=LudoGameTesting2");
            optionsBuilder.UseSqlServer(@"Server=localhost,1433;Initial Catalog=LudoGame;User Id=sa;Password=verystrong!pass123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // because we have 1 to many relationships, something goes wrong when you try to add migration, and also update to the database, so, here, you 
            // are basically telling the database, there can be one to many.
            modelBuilder.Entity<DbBoardState>()
                .HasOne(entity => entity.Player)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DbBoardState>()
                .HasOne(entity => entity.Board)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<DbBoardState>()
            //    .HasOne(entity => entity.Color)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DbPlayer>()
                .HasOne(entity => entity.Color)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DbWinner>()
                .HasOne(entity => entity.Player)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DbWinner>()
                .HasOne(entity => entity.Board)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            #region Colors
            modelBuilder.Entity<DbColor>().HasData(
                new DbColor { Id = 1, ColorCode = "" },
                new DbColor { Id = 2, ColorCode = "" },
                new DbColor { Id = 3, ColorCode = "" },
                new DbColor { Id = 4, ColorCode = "" }
            );
            #endregion

            #region Board
            modelBuilder.Entity<DbBoard>().HasData(
                new DbBoard { Id = 1, LastTimePlayed = DateTime.Parse("2021-03-29 21:55:05"), IsFinished = true },
                new DbBoard { Id = 2, LastTimePlayed = DateTime.Parse("2021-01-15 21:55:05"), IsFinished = true },
                new DbBoard { Id = 3, LastTimePlayed = DateTime.Parse("2021-05-02 21:55:05"), IsFinished = true },
                new DbBoard { Id = 4, LastTimePlayed = DateTime.Parse("2021-02-25 21:55:05"), IsFinished = true },
                new DbBoard { Id = 5, LastTimePlayed = DateTime.Parse("2021-03-21 21:55:05"), IsFinished = false }

            );
            #endregion

            #region BoardState
            modelBuilder.Entity<DbBoardState>().HasData(
                new DbBoardState { Id = 1, PlayerId = 1, BoardId = 5, PieceNumber = 1, PiecePosition = 18, IsInSafeZone = false, IsInBase = false },
                new DbBoardState { Id = 2, PlayerId = 1, BoardId = 5, PieceNumber = 2, PiecePosition = 13, IsInSafeZone = false, IsInBase = false },
                new DbBoardState { Id = 3, PlayerId = 1, BoardId = 5, PieceNumber = 3, PiecePosition = 0, IsInSafeZone = false, IsInBase = true },
                new DbBoardState { Id = 4, PlayerId = 1, BoardId = 5, PieceNumber = 4, PiecePosition = 0, IsInSafeZone = false, IsInBase = true },
                new DbBoardState { Id = 5, PlayerId = 2, BoardId = 5, PieceNumber = 1, PiecePosition = 0, IsInSafeZone = true, IsInBase = false },
                new DbBoardState { Id = 6, PlayerId = 2, BoardId = 5, PieceNumber = 2, PiecePosition = 22, IsInSafeZone = false, IsInBase = false },
                new DbBoardState { Id = 7, PlayerId = 2, BoardId = 5, PieceNumber = 3, PiecePosition = 0, IsInSafeZone = false, IsInBase = true },
                new DbBoardState { Id = 8, PlayerId = 2, BoardId = 5, PieceNumber = 4, PiecePosition = 0, IsInSafeZone = false, IsInBase = true }
                );
            #endregion

            #region Winner
            modelBuilder.Entity<DbWinner>().HasData(
                new DbWinner { Id = 1, BoardId = 1, PlayerId = 2, Placement = 1 },
                new DbWinner { Id = 2, BoardId = 1, PlayerId = 1, Placement = 2 },
                new DbWinner { Id = 3, BoardId = 2, PlayerId = 3, Placement = 2 },
                new DbWinner { Id = 4, BoardId = 2, PlayerId = 4, Placement = 3 },
                new DbWinner { Id = 5, BoardId = 2, PlayerId = 6, Placement = 4 },
                new DbWinner { Id = 6, BoardId = 2, PlayerId = 5, Placement = 1 }
            );
            #endregion

            #region Player
            modelBuilder.Entity<DbPlayer>().HasData(
                new DbPlayer { Id = 1, Name = "Lisa", ColorId = 2 },
                new DbPlayer { Id = 2, Name = "Bobby", ColorId = 1 },
                new DbPlayer { Id = 3, Name = "Anna", ColorId = 3 },
                new DbPlayer { Id = 4, Name = "Luke", ColorId = 4 },
                new DbPlayer { Id = 5, Name = "Roseline", ColorId = 1 },
                new DbPlayer { Id = 6, Name = "Noa", ColorId = 2 }
            );
            #endregion
        }

    }
}
