using Academy.Data;
using Academy.Services;
using Academy.Services.Contracts;
using Academy.Web.Areas.Teacher.Controllers;
using Academy.Web.Areas.Teacher.Models;
using Academy.Web.Utilities.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Tests
{
    [TestClass]
    public class TeacherAreaTests
    {
        [TestMethod]
        public async Task DashboardShouldReturnViewWithCorrectModel()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            var result = await controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var view = (ViewResult)result;
            Assert.IsInstanceOfType(view.Model, typeof(CoursesByTeacherViewModel));

        }

        [TestMethod]
        public async Task DashboardShouldCallCorrectServiceMethodOnce()
        {
            var userService = new Mock<IUserService>();
            var courseService = new Mock<ICourseService>();

            userService.Setup(us => us.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User());
            courseService.Setup(cs => cs.RetrieveCoursesByTeacherAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Course>());

            var controller = new DashboardController
                (courseService.Object, userService.Object, this.SetupWrapperForInvokeTests());
            var result = await controller.Index();

            courseService.Verify(cs => cs.RetrieveCoursesByTeacherAsync(1), Times.Once);
        }

        private DashboardController SetupControllerForAuthenticationConnectTests()
        {
            var courseService = new Mock<ICourseService>();
            courseService.Setup(cs => cs.RetrieveCoursesByTeacherAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Course>() { new Course() { Name = "Course1" }, new Course() { Name = "Course2" } });

            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User() { UserName = "Pesho" });

            var userWrapper = new Mock<IUserWrapper>();
            userWrapper.Setup(us => us.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            var controller = new DashboardController(courseService.Object, userService.Object,userWrapper.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = new ClaimsPrincipal()
                    }
                },
                TempData = new Mock<ITempDataDictionary>().Object
            };


            return controller;
        }

        private IUserWrapper SetupWrapperForInvokeTests()
        {
            var userWrapper = new Mock<IUserWrapper>();
            userWrapper.Setup(us => us.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            return userWrapper.Object;
        }
    }


}
