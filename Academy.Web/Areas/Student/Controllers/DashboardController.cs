using System.Linq;
using System.Threading.Tasks;
using Academy.Data;
using Academy.Services.Contracts;
using Academy.Web.Areas.Student.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Student.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICourseService courseService;
        private readonly IUserService userService;

        public DashboardController(UserManager<User> userManager, ICourseService courseService, IUserService userService)
        {
            this.userManager = userManager;
            this.courseService = courseService;
            this.userService = userService;
        }

        [Area("Student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var studentId = int.Parse(this.userManager.GetUserId(HttpContext.User));
            var user = await this.userService.GetUserByIdAsync(studentId);
            var courses = await this.courseService.RetrieveCoursesByStudentAsync(studentId);
            var model = new CoursesByStudentViewModel()
            {
                UserName = user.FullName,
                Courses = courses.Select(co => new CourseViewModel(co))
            };

            return View(model);
        }

        [Area("Teacher")]
        [Authorize(Roles = "Teacher")]
        public IActionResult Add()
        {
            return this.View();
        }
    }
}