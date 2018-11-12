using Academy.Data;
using Academy.Services.Contracts;
using Academy.Web.Areas.Administration.Controllers;
using Academy.Web.Areas.Administration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Tests
{
    [TestClass]
    public class AdministrationAreaTests
    {
        [TestMethod]
        public async Task DashboardShouldReturnViewWithCorrectModel()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            var result = await controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var view = (ViewResult)result;
            Assert.IsInstanceOfType(view.Model, typeof(AdminViewModel));
        }

        [TestMethod]
        public async Task DashboardShouldCallCorrectServiceMethodOnce()
        {
            var userService = new Mock<IUserService>();

            userService.Setup(us => us.UpdateRoleAsync(It.IsAny<int>(), 2))
                .Returns(Task.FromResult(true))
                .Verifiable();

            var controller = new DashboardController(userService.Object);

            var moqTempData = new Mock<ITempDataDictionary>();

            moqTempData.Setup(td => td[It.IsAny<string>()])
                .Returns("test");

            controller.TempData = moqTempData.Object;

            var model = new AdminViewModel()
            {
                userId = 2,              
            };

            var result = await controller.Promote(model);

            userService.Verify(us => us.UpdateRoleAsync(2, 2), Times.Once);
        }

        private DashboardController SetupControllerForAuthenticationConnectTests()
        {
            var userService = new Mock<IUserService>();
            userService.Setup(us => us.UpdateRoleAsync(It.IsAny<int>(), 2));

            var controller = new DashboardController(userService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal()
                    }
                },

            };

            return controller;
        }
    }
}
