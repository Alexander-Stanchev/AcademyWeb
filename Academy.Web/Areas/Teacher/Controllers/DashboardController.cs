using System.Linq;
using System.Threading.Tasks;
using Academy.Data;
using Academy.Services.Contracts;
using Academy.Web.Areas.Teacher.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Teacher.Controllers
{

    [Area("Teacher")]
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

        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(this.userManager.GetUserId(HttpContext.User));
            var user = await this.userService.GetUserByIdAsync(userId);
            var courses = await this.courseService.RetrieveCoursesByTeacherAsync(userId);
            var model = new CoursesByTeacherViewModel()
            {
                UserName = user.FullName,
                Courses = courses.Select(co => new CourseViewModel(co))
            };

            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Course(int id)
        {
            var course = await this.courseService.GetCourseByIdAsync(id);
            var model = new CourseInformationModel()
            {
                Course = new CourseViewModel(course),
                Assignments = course.Assignments.Select(a => new AssignmentViewModel(a))
            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles ="Teacher")]
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles ="Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = int.Parse(this.userManager.GetUserId(HttpContext.User));
                var course = await this.courseService.AddCourseAsync(userId, model.Start, model.End, model.Name);
                TempData["Success"] = $"Successfully added course {course.Name}";
                return this.RedirectToAction("course", "dashboard", new { id = course.CourseId });
            }
            return this.View(model);

        }

        [HttpPost]
        [Authorize(Roles ="Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GradeStudent(GradeViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = int.Parse(this.userManager.GetUserId(HttpContext.User));
                await this.userService.EvaluateStudentAsync(model.StudentId, model.AssignmentId, (int)model.PointsReceived, userId);
                return Json(new { status = "true" });
            }
            else
            {
                return Json(new { status = "false"});
            }
        }

    }
}