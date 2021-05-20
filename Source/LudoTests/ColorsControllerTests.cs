using LudoApi.Controllers;
using LudoApi.DTOs;
using LudoEngine.Database;
using LudoEngine.Engine;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LudoTests
{
    public class ColorsControllerTests
    {
        [Fact]
        public async Task Add_4_Colors_Expect_Count_4()
        {
            await using var context = new TestContext();
            context.SeedColors();

            var colorsController = new ColorsController(context);
            var actionResult = await colorsController.GetColors();
            var colors = actionResult.Value as List<ColorDTO>;

            Assert.Equal(4, colors.Count);
            Assert.Equal(2, colors[1].Id);
            Assert.Equal("FF0000", colors[0].ColorCode);
        }

        [Fact]
        public async Task Add_4_Colors_Expect_GetDbColor_Return_Id_3()
        {
            await using var context = new TestContext();
            context.SeedColors();

            var colorsController = new ColorsController(context);
            var actionResult = await colorsController.GetDbColor(3);
            var color = actionResult.Value;

            Assert.Equal(3, color.Id);
            Assert.Equal("584147", color.ColorCode);
        }

        [Fact]
        public async Task Post_1_Color_Expect_Identical_Results()
        {
            await using var context = new TestContext();
            var colorsController = new ColorsController(context);

            var dbColor = new DbColor() { ColorCode = "69869c" };
            var actionResult = await colorsController.PostDbColor(dbColor);
            ColorDTO color = (ColorDTO)(actionResult.Result as CreatedAtActionResult).Value;

            Assert.Equal(dbColor.ColorCode, color.ColorCode);
        }

        [Fact]
        public async Task Post_1_Color_Expect_Iden()
        {
            await using var context = new TestContext();
            context.SeedColors();

            var colorsController = new ColorsController(context);
            var color = await context.Colors.FindAsync(4);
            Assert.NotNull(color);

            var colorToBeDeleted = await colorsController.DeleteDbColor(4);
            color = await context.Colors.FindAsync(4);
            Assert.Null(color);
        }

        [Fact]
        public async Task Patch_1_Color_Expect_A_New_String()
        {
            await using var context = new TestContext();
            context.SeedColors();

            var patchDoc = new JsonPatchDocument<DbColor>();
            patchDoc.Replace(color => color.ColorCode, "C0FFEE");
            var colorController = new ColorsController(context);
            var patchedColor = await colorController.PatchColor(4, patchDoc);

            var objectResult = (ObjectResult)patchedColor;
            var resultColor = (DbColor)objectResult.Value;
            Assert.Equal("C0FFEE", resultColor.ColorCode);
        }
    }
}
