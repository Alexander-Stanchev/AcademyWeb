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
                .ReturnsAsync(new List<Course>() { new Course() { CourseId = 1, Name = "Mocking" } });

            var controller = new DashboardController
                (courseService.Object, userService.Object, this.SetupWrapperForInvokeTests());
            var result = await controller.Index();

            courseService.Verify(cs => cs.RetrieveCoursesByTeacherAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task CourseShouldCallCorrectServiceMethodOnce()
        {
            var userService = new Mock<IUserService>();
            var courseService = new Mock<ICourseService>();

            userService.Setup(us => us.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User());
            courseService.Setup(cs => cs.RetrieveCoursesByTeacherAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Course>() { new Course() { CourseId = 1, Name = "Mocking" } });

            var controller = new DashboardController(courseService.Object, userService.Object, this.SetupWrapperForInvokeTests());

            var result = await controller.Course(1);

            courseService.Verify(cs => cs.GetCourseByIdAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task CourseShouldReturnViewWithCorrectModel()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            var result = await controller.Course(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var view = (ViewResult)result;
            Assert.IsInstanceOfType(view.Model, typeof(CourseInformationModel));
        }

        [TestMethod]
        public async Task CourseShouldRedirectToNotFoundIfCourseNotFound()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            var result = await controller.Course(111);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void AddGetMethodShouldReturnCorrectType()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            var result = controller.Add();

            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public async Task AddPostMethodShouldReturnCorrectIfModelInvalid()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            controller.ModelState.AddModelError("error", "error");

            var viewModel = new CourseViewModel();

            var result = await controller.Add(viewModel);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(CourseViewModel));
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task AddPostMethodShouldRedirectIfModelValid()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            var viewModel = new CourseViewModel()
            {

            };

            var result = await controller.Add(viewModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("dashboard", redirectResult.ControllerName);
            Assert.AreEqual("course", redirectResult.ActionName);
            Assert.AreEqual(1, redirectResult.RouteValues["id"]);

        }

        [TestMethod]
        public async Task GradeStudentShouldReturnFalseJsonIfInvalidModel()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            controller.ModelState.AddModelError("error", "error");

            var model = new GradeViewModel();

            var result = await controller.GradeStudent(model);

            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            Assert.IsTrue(jsonResult.Value.ToString() == "{ status = false }");
        }

        [TestMethod]
        public async Task GradeStudentShouldReturnTrueIfValidModel()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();

            var model = new GradeViewModel()
            {
                StudentId = 1
            };

            var result = await controller.GradeStudent(model);

            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var jsonResult = (JsonResult)result;
            var str = jsonResult.Value.ToString();
            Assert.IsTrue(jsonResult.Value.ToString() == "{ status = true, studentId = 1 }");
        }

        [TestMethod]
        public void NewAssignmentShouldReturnProperView()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            var result = controller.NewAssignment(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));


        }

        [TestMethod]
        public async Task NewAssignmentPostShouldReturnViewIfInvalidModel()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();
            controller.ModelState.AddModelError("error", "error");

            var model = new AssignmentViewModel();
            var result = await controller.NewAssignment(model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(AssignmentViewModel));
            Assert.IsNull(viewResult.ViewName);

        }

        [TestMethod]
        public async Task NewAssignmentShouldRedirectCorrectlyIfModelValid()
        {
            var controller = this.SetupControllerForAuthenticationConnectTests();

            var model = new AssignmentViewModel()
            {
                CourseId = 1
            };

            var result = await controller.NewAssignment(model);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("dashboard", redirectResult.ControllerName);
            Assert.AreEqual("course", redirectResult.ActionName);
            Assert.AreEqual(1, redirectResult.RouteValues["id"]);
        }

        //HELPERS START HERE
        private DashboardController SetupControllerForAuthenticationConnectTests()
        {
            var courseService = new Mock<ICourseService>();
            courseService.Setup(cs => cs.RetrieveCoursesByTeacherAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<Course>() { new Course() { CourseId = 1,Name = "Course1" }, new Course() { Name = "Course2" } });

            courseService.Setup(cs => cs.GetCourseByIdAsync(It.IsInRange<int>(1,110,Range.Inclusive)))
                .ReturnsAsync(new Course()
                {
                    CourseId = 1,
                    EnrolledStudents = new List<EnrolledStudent>(),
                    Name = "Mock",
                    Assignments = new List<Assignment>()
                });

            courseService.Setup(cs => cs.AddCourseAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(new Course() { CourseId = 1 });

            courseService.Setup(cs => cs.AddAssignment(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new Assignment()
                {
                    Name = "MockAssignment", 
                    Id = 1,
                    CourseId = 1,
                    Course = new Course()
                    {
                        Name = "MockCourse"
                    }
                });
           

            var userService = new Mock<IUserService>();
            userService.Setup(us => us.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new User() { UserName = "Pesho" });

            var userWrapper = new Mock<IUserWrapper>();
            userWrapper.Setup(us => us.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");

            var controller = new DashboardController(courseService.Object, userService.Object, userWrapper.Object)
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
