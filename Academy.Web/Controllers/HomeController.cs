using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Academy.Web.Models;
using Academy.Services.Contracts;
using Academy.Web.Models.HomeViewModels;
using Microsoft.Extensions.Caching.Memory;
using Academy.Data;

namespace Academy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IMemoryCache cacheService;
        public HomeController(ICourseService courseService, IMemoryCache cacheService)
        {
            this.courseService = courseService;
            this.cacheService = cacheService;
        }
        public async Task<IActionResult> Index()
        {

            var courses = await this.GetCachedCourses();
            var model = new HomeViewModel()
            {
                Courses = courses.Select(co => new CourseViewModel(co))
            };
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("InternalServerError", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IEnumerable<Course>> GetCachedCourses()
        {
            var cacheCourses = await cacheService.GetOrCreateAsync("Data", async entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddSeconds(60);
                var courses = await this.courseService.GetAllCoursesAsync();
                return courses;
            });
            return cacheCourses;
        }
    }
}
