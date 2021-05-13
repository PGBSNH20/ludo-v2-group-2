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

namespace LudoTests
{
    public class Test
    {
        //protected readonly HttpClient TestClient;

        //protected Test()
        //{
        //    var appFactory = new WebApplicationFactory<Startup>()
        //        .WithWebHostBuilder(builder =>
        //        {
        //            builder.ConfigureServices(services =>
        //            {
        //                services.RemoveAll(typeof(LudoContext));
        //                services.AddDbContext<LudoContext>(optionsAction: options => { options.UseInMemoryDatabase(databaseName: "TestDb"); });
        //            });
        //        });

        //    TestClient = appFactory.CreateClient();
        //}
    }
    public class UnitTest1 : Test
    {



        //[Fact]
        //public async void Test1()
        //{
        //   var response = await TestClient.GetAsync("https://localhost:5001/api/players");



        //    response.StatusCode.Should().Be(HttpStatusCode.OK);

        //}
    }
}
