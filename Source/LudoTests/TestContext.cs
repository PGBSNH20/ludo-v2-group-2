using LudoEngine.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoTests
{
    class TestContext : LudoContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // guid, is gobally unique identifier we are trying to get a new random database name so we don't have to delete the changes after every test
            var guid = Guid.NewGuid().ToString();
            optionsBuilder.UseInMemoryDatabase(databaseName: guid);
        }

        public void SeedBoards()
        {
            Add(new DbBoard { Id = 1, LastTimePlayed = DateTime.Parse("2021-03-29 21:55:05"), IsFinished = true });
            Add(new DbBoard { Id = 2, LastTimePlayed = DateTime.Parse("2021-04-01 21:55:05"), IsFinished = false });
            Add(new DbBoard { Id = 3, LastTimePlayed = DateTime.Parse("2021-05-13 21:55:05"), IsFinished = true });
            SaveChanges();
        }

        public void SeedPlayers()
        {
            Add(new DbPlayer { Id = 1, Name = "Allie", ColorId = 1 });
            Add(new DbPlayer { Id = 2, Name = "Bobbie", ColorId = 2 });
            Add(new DbPlayer { Id = 3, Name = "Duckie", ColorId = 1 });
            Add(new DbPlayer { Id = 4, Name = "Annie", ColorId = 3 });
            SaveChanges();
        }

        public void SeedWinners()
        {
            Add(new DbWinner { Id = 1, PlayerId = 1, BoardId = 1, Placement = 1 });
            Add(new DbWinner { Id = 2, PlayerId = 2, BoardId = 1, Placement = 2 });
            Add(new DbWinner { Id = 3, PlayerId = 3, BoardId = 3, Placement = 2 });
            Add(new DbWinner { Id = 4, PlayerId = 4, BoardId = 3, Placement = 1 });
            SaveChanges();
        }

        public void SeedBoardStates()
        {
            Add(new DbBoardState { Id = 1, PlayerId = 1, BoardId = 2, PieceNumber = 1, PiecePosition = 45, IsInSafeZone = false, IsInBase = false });
            Add(new DbBoardState { Id = 2, PlayerId = 1, BoardId = 2, PieceNumber = 2, PiecePosition = 13, IsInSafeZone = false, IsInBase = false });
            Add(new DbBoardState { Id = 3, PlayerId = 1, BoardId = 2, PieceNumber = 3, PiecePosition = 0, IsInSafeZone = false, IsInBase = true });
            Add(new DbBoardState { Id = 4, PlayerId = 1, BoardId = 2, PieceNumber = 4, PiecePosition = 0, IsInSafeZone = false, IsInBase = true });
            Add(new DbBoardState { Id = 5, PlayerId = 2, BoardId = 2, PieceNumber = 1, PiecePosition = 1, IsInSafeZone = true, IsInBase = false });
            Add(new DbBoardState { Id = 6, PlayerId = 2, BoardId = 2, PieceNumber = 2, PiecePosition = 22, IsInSafeZone = false, IsInBase = false });
            Add(new DbBoardState { Id = 7, PlayerId = 2, BoardId = 2, PieceNumber = 3, PiecePosition = 11, IsInSafeZone = false, IsInBase = false });
            Add(new DbBoardState { Id = 8, PlayerId = 2, BoardId = 2, PieceNumber = 4, PiecePosition = 0, IsInSafeZone = false, IsInBase = true });
        }

        public void SeedHistory()
        {
            SeedBoards();
            SeedPlayers();
            SeedWinners();
        }

        public void SeedUnfinishedBoards()
        {
            SeedBoards();
            SeedPlayers();
            SeedBoardStates();

        }
    }
}
