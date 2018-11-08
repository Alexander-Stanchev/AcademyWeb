using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Academy.Data;
using Academy.Services.Contracts;
using Academy.Web.Areas.Administration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.Administration.Controllers
{

    public class DashboardController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IUserService userService;

        public DashboardController(UserManager<User> userManager, IUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        [Area("Administration")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {            
            var users = await this.userService.RetrieveUsers(3);

            var model = new AdminViewModel()
            {
                Users = users.Select(us => new UsersViewModel(us))
            };
            return View(model);
        }

        [HttpPost]
        [Area("Administration")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Promote(AdminViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await userService.UpdateRoleAsync(model.userId, 2);

                TempData["UserMessage"] = "Congrajulationz";

                return this.RedirectToAction("index", "dashboard");
            }
            return View(model);
        }
    }
}