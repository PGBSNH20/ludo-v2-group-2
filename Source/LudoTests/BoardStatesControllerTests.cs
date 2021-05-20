using LudoApi.Controllers;
using LudoApi.DTOs;
using LudoEngine.Database;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LudoTests
{
    public class BoardStatesControllerTests
    {
        [Fact]
        public async Task GetBoardStates_Expect_Count_8()
        {
            await using var context = new TestContext();
            var controller = new BoardStatesController(context);
            context.SeedBoardStates();

            var actionResult = await controller.GetBoardStates();
            var boardStates = actionResult.Value as List<BoardStateDTO>;

            Assert.Equal(8, boardStates.Count);
        }

        [Fact]
        public async Task GetBoardState_4_Expect_Equal_SeedBoardStates_Values()
        {
            await using var context = new TestContext();
            var controller = new BoardStatesController(context);
            context.SeedBoardStates();

            var actionResult = await controller.GetBoardState(4);
            var boardState = actionResult.Value;

            Assert.Equal(4, boardState.Id);
            Assert.Equal(1, boardState.PlayerId);
            Assert.Equal(2, boardState.BoardId);
            Assert.Equal(4, boardState.PieceNumber);
            Assert.Equal(0, boardState.PiecePosition);
            Assert.False(boardState.IsInSafeZone);
            Assert.True(boardState.IsInBase);
        }

        [Fact]
        public async Task GetAllBoardStatesByBoardId_2_Expect_8_BoardStates()
        {
            await using var context = new TestContext();
            var controller = new BoardStatesController(context);
            context.SeedBoardStates();

            var actionResult = await controller.GetAllBoardStatesByBoardId(2);
            var boardStates = actionResult.Value;

            Assert.Equal(8, boardStates.Count);
        }

        [Fact]
        public async Task GetAllBoardStatesForPlayerId_Board_2_Player_1_Expect_4_BoardStates()
        {
            await using var context = new TestContext();
            var controller = new BoardStatesController(context);
            context.SeedBoardStates();

            var actionResult = await controller.GetAllBoardStatesForPlayerId(2, 1);
            var boardStates = actionResult.Value;

            Assert.Equal(4, boardStates.Count);
        }

        [Fact]
        public async Task Patch_1_BoardState_Expect_Replaced_String()
        {
            await using var context = new TestContext();
            context.SeedBoardStates();

            var patchDoc = new JsonPatchDocument<DbBoardState>();
            patchDoc.Replace(state => state.PiecePosition, 24);
            var controller = new BoardStatesController(context);
            var patchedState = await controller.PatchBoardState(6, patchDoc);

            var objectResult = (ObjectResult)patchedState;
            var resultColor = (DbBoardState)objectResult.Value;
            Assert.Equal(24, resultColor.PiecePosition);
        }

        [Fact]
        public async Task DeleteDbBoardState_1_and_2_Expect_6_BoardStates()
        {
            await using var context = new TestContext();
            var controller = new BoardStatesController(context);
            context.SeedBoardStates();

            Assert.Equal(8, context.BoardStates.Count());
            await controller.DeleteDbBoardState(1);
            await controller.DeleteDbBoardState(2);
            Assert.Equal(6, context.BoardStates.Count());
        }
    }
}
