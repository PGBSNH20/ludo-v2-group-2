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

namespace LudoTests
{
    
    public class ControllerTests
        
    {
        private DbContext _context;
        private PlayersController _controller;
        public void Setup()
        {
            var dbContextOptions =
                new DbContextOptionsBuilder<LudoContext>().UseInMemoryDatabase(databaseName: "TestDB");
            _context = new LudoContext();
            _context.Database.EnsureCreated();

            _controller = new PlayersController((LudoContext)_context);
        }

        public void Close()
        {
            _context.Database.EnsureDeleted();
        }

        [Fact]
        public async Task GetAllSpacePorts()
        {
            Setup();          
            var result = await _controller.GetPlayers();
            Assert.NotNull(result.Value);
            Close();
        }
    }
}
