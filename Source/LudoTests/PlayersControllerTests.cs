using LudoApi;
using LudoApi.Controllers;
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

//namespace LudoTests
//{
//    public class PlayersControllerTests
//    {
//        private DbContext _context;
//        private PlayersController _controller;
        
//        private void Setup()
//        {
//            var dbContextOptions = new DbContextOptionsBuilder<LudoContext>().UseInMemoryDatabase(databaseName: "TestDB");
//            _context = new LudoContext();
//            _context.Database.EnsureCreated();
//            _controller = new PlayersController((LudoContext)_context);
//        }

//        private void Close()
//        {
//            _context.Database.EnsureDeleted();
//        }

//        [Fact]
//        public async Task Get_AllPlayers_Expect_True()
//        {
//            Setup();
//            bool check = false;
//            var result = await _controller.GetPlayers();
//            foreach (var item in result.Value)
//            {
//                if (item.Name.Contains("Anna"))
//                {
//                    check = true;
//                }
//            }
//            Assert.True(check);
//            Close();
//        }

//        [Fact]
//        public async Task Get_PlayerById_Expect_True()
//        {
//            Setup();
//            bool check = false;
//            var result = await _controller.GetDbPlayer(1);

//            if (result.Value.Name.Contains("Lisa"))
//            {
//                check = true;
//            }

//            Assert.True(check);
//            Close();
//        }

//        [Fact]
//        public async Task Put_Modify_A_PlayerDetails_Expect_True()
//        {
//            Setup();
//            bool check = false;
//            DbPlayer player = new DbPlayer() { Name = "Nancy", ColorId = 2, Id = 1 };
//            await _controller.PutDbPlayer(1, player);
//            var validate = await _controller.GetDbPlayer(1);
//            if (validate.Value.Name == "Nancy")
//            {
//                check = true;
//            }
//            Assert.True(check);
//            Close();
//        }
        
//        [Fact]
//        public async Task Post_AddNewPlayer_Expect_True()
//        {
//            Setup();
//            DbPlayer player = new DbPlayer() { Name = "Oskar", ColorId = 2 };
//            await _controller.PostDbPlayer(player);

//            var result = await _controller.GetDbPlayer(7);
//            Assert.True(result.Value.Name=="Oskar");
//            Close();
//        }

//        [Fact]
//        public async Task Delete_DeletePlayer_byId_Expect_False()
//        {
//            Setup();
//            DbPlayer player = new DbPlayer() { Name = "Oskar", ColorId = 2 };
//            await _controller.PostDbPlayer(player);
//            await _controller.DeleteDbPlayer(7);
//            var result = await _controller.GetDbPlayer(7);
//            Assert.True(result.Value==null);
//            Close();
//        }
//    }
//}
