using LudoApi.Controllers;
using LudoApi.DTOs;
using LudoEngine.Database;
using LudoEngine.Engine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LudoTests
{
    public class BoardsControllerTest
    {
        [Fact]
        public async Task Add_3_Boards_Expect_Count_3_And_Values_Equal()
        {
            await using var context = new TestContext();
            context.SeedBoards();

            var boardsController = new BoardsController(context);
            var boardActionResult = await boardsController.GetBoards();
            List<BoardDTO> boards = boardActionResult.Value as List<BoardDTO>;

            Assert.Equal(3, boards.Count);
            Assert.Equal(2, boards[1].Id);
            Assert.Equal("2021-03-29 21:55:05", boards[0].LastTimePlayed.ToString());
            Assert.True(boards[2].IsFinished);
        }

        [Fact]
        public async Task Add_3_Boards_Expect_GetDbBoard_Returns_Board_2()
        {
            await using var context = new TestContext();
            context.SeedBoards();

            var boardsController = new BoardsController(context);
            var boardActionResult = await boardsController.GetDbBoard(2);
            BoardDTO board = boardActionResult.Value;

            Assert.Equal(2, board.Id);
            Assert.Equal("2021-04-01 21:55:05", board.LastTimePlayed.ToString());
            Assert.False(board.IsFinished);
        }

        [Fact]
        public async Task Add_3_Boards_Expect_GetDbBoardHistory_Returns_2_Boards()
        {
            await using var context = new TestContext();
            context.SeedHistory();

            var boardsController = new BoardsController(context);
            var boardActionResult = await boardsController.GetDbBoardsHistory();
            var history = boardActionResult.Value;

            Assert.Equal(2, history.Count);
            Assert.Equal("2021-03-29 21:55:05", history[0].LastTimePlayed.ToString());
            Assert.Equal(2, history[0].Placements.Count);
            Assert.Equal("Allie", history[0].Placements[1]);
            Assert.Equal(1, history[0].GameId);
        }

        [Fact]
        public async Task Add_3_Boards_Expect_GetDbUnfinishedBoards_Returns_1_Board()
        {
            await using var context = new TestContext();
            context.SeedUnfinishedBoards();

            var boardsController = new BoardsController(context);
            var boardActionResult = await boardsController.GetDbUnfinishedBoards();
            var unfinishedBoards = boardActionResult.Value;

            Assert.Single(unfinishedBoards);
            Assert.Equal(2, unfinishedBoards[0].BoardId);
            Assert.Equal("2021-04-01 21:55:05", unfinishedBoards[0].LastTimePlayed.ToString());
            Assert.Contains("Allie", unfinishedBoards[0].PlayerNames);
        }

        //[Fact]
        //public async Task Post_1_Board_Expect_()
        //{
        //    await using var context = new TestContext();

        //    var boardsController = new BoardsController(context);
        //    var boardActionResult = await boardsController.PostDbBoard();
        //    BoardDTO board = boardActionResult.Value;

        //    Assert.Equal(2, board.Id);
        //    Assert.Equal("2021-04-01 21:55:05", board.LastTimePlayed.ToString());
        //    Assert.False(board.IsFinished);
        //}
    }
}
