using System;
using System.Collections.Generic;
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

        [Area("Teacher")]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(this.userManager.GetUserId(HttpContext.User));
            var courses = await this.courseService.GetAllCoursesAsync();
            var model = new CoursesByTeacherViewModel()
            {
                UserName = userId,
                Courses = courses.Select(co => new CourseViewModel(co))
            };

            return View(model);
        }
    }
}