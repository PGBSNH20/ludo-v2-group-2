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
            bool check = false;
            Setup();
            var result = await _controller.GetPlayers();            
            foreach (var item in result.Value)
            {
                if (item.Name.Contains("Lisa"))
                {
                    check = true;
                }
            }            
            Assert.True(check);
            Close();          
        }
    }
}
