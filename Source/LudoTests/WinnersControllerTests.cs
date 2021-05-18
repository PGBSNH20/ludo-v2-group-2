using LudoApi;
using LudoApi.Controllers;
using LudoEngine.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FluentAssertions;
using System.Net;
using FakeItEasy;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Assert = Xunit.Assert;
using LudoApi.DTOs;

namespace LudoTests
{

    public class WinnersControllerTests

    {
        private DbContext _context;
        private WinnersController _controller;
        public void Setup()
        {
            var dbContextOptions =
                new DbContextOptionsBuilder<LudoContext>().UseInMemoryDatabase(databaseName: "TestDB");
            _context = new LudoContext();
            _context.Database.EnsureCreated();

            _controller = new WinnersController((LudoContext)_context);
        }

        public void Close()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task Get_AllWinners_Expect_True()
        {

            Setup();
            bool check = false;
            var result = await _controller.GetWinners();
            foreach (var item in result.Value)
            {
                if (item != null)
                {
                    check = true;
                }
            }
            Assert.True(check);
            Close();
        }

        [Fact]
        public async Task Get_WinnerById_Expect_True()
        {
            Setup();
            bool check = false;
            var result = await _controller.GetDbWinner(1);

            if (result.Value.PlayerId == 1)
            {
                check = true;
            }

            Assert.True(check);
            Close();
        }

        [Fact]
        public async Task Put_Modify_A_WinnerDetails_Expect_True()
        {
            Setup();
            bool check = false;
            DbWinner player = new DbWinner() { BoardId=3,PlayerId=6,Placement=1 };
            await _controller.PutDbWinner(1, player);
            var validate = await _controller.GetDbWinner(6);
            if (validate.Value.Placement ==1 )
            {
                check = true;
            }
            Assert.True(check);
            Close();
        }

        [Fact]
        public async Task Post_AddNewWinner_Expect_True()
        {
            Setup();
            bool check = false;
            DbWinner player = new DbWinner() { PlayerId = 6, BoardId = 3, Placement = 1 };
            await _controller.PostDbWinner(player);
            var validate = await _controller.GetDbWinner(6);
            if (validate.Value.Placement == 1)
            {
                check = true;
            }
            Assert.True(check);
            Close();
        }

        [Fact]
        public async Task Delete_DeleteWinner_byId_Expect_True()
        {
            Setup();
            bool check = false;
          //  DbWinner player = new DbWinner() { PlayerId = 6, BoardId = 3, Placement = 1 };
            await _controller.DeleteDbWinner(6);
            var validate = await _controller.GetDbWinner(6);
            if (validate.Value==null)
            {
                check = true;
            }
            Assert.True(check);
            Close();
        }
    }
}
