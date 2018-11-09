using Academy.Data;
using Academy.Services;
using Academy.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Tests
{
    [TestClass]
    public class TeacherAreaTests
    {
        [TestMethod]
        public async Task DashboardShouldCallUserServiceMethod()
        {

        }

        private Mock<ICourseService> SetupCourseServiceMock()
        {
            var courseServiceMock = new Mock<ICourseService>();
            courseServiceMock.Setup(cs => cs.GetAllCoursesAsync().GetAwaiter().GetResult())
                .Returns(new List<Course>());

            return courseServiceMock;
        }

        private Mock<IUserService> SetupUserServiceMock()
        {
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(us => us.GetUserByIdAsync(It.IsAny<int>()).GetAwaiter().GetResult())
                .Returns(new User { Id = 1, RoleId = 2, UserName = "Gosho" });
            return userServiceMock;
        }
    }


}
