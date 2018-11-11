using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Academy.Data;
using Academy.Services.Contracts;
using Academy.Services.Providers.Abstract;
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
        private readonly IGradeService gradeService;
        private readonly IExporter exporter;

        public DashboardController(UserManager<User> userManager, ICourseService courseService, IUserService userService, IGradeService gradeService,  IExporter exporter)
        {
            this.userManager = userManager;
            this.courseService = courseService;
            this.userService = userService;
            this.gradeService = gradeService;
            this.exporter = exporter;
        }

        [Area("Student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var studentId = int.Parse(this.userManager.GetUserId(HttpContext.User));
            var user = await this.userService.GetUserByIdAsync(studentId);
            var courses = await this.courseService.RetrieveCoursesByStudentAsync(studentId);
            var grades = user.Grades.ToList();


            var model = new CoursesByStudentViewModel()
            {
                UserName = user.FullName,
                Courses = courses.Select(co => new CourseViewModel(co)),
                Grades = grades.Select(gr => new GradeViewModel(gr))

            };

            return View(model);
        }

        [Area("Student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Assignments(int Id)
        {
            var userId = int.Parse(this.userManager.GetUserId(HttpContext.User));
            var user =  await userService.GetUserByIdAsync(userId);
            var assignments = await courseService.RetrieveAssignmentsGradesForStudentAsync(Id, userId);
            var grades = user.Grades.ToList();
            var model = new AssignmentByStudentViewModel()
            {
                UserName = user.FullName,
                Assignments = assignments.Select(ass => new AssignmentViewModel(ass)),
                Grades = grades.Select(gr => new GradeViewModel(gr))
            };

            return View(model);
        }

        [Area("Student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ExportToPDF(int Id)
        { 
            if (this.ModelState.IsValid)
            {
                var grades = await this.gradeService.RetrieveGradesAsync(this.userManager.GetUserName(HttpContext.User));

                this.exporter.GenerateReport(grades, this.userManager.GetUserName(HttpContext.User));                
            }
            
            return Ok("PDF downloaded");
        }
    }
}