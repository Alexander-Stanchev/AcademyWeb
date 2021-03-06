﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Academy.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Academy.DataContext;
using Academy.Data;
using Academy.Services;
using Academy.Services.Contracts;
using Academy.Services.Providers.Abstract;
using Academy.Services.Providers;
using Newtonsoft.Json.Serialization;
using Academy.Web.Utilities;
using Academy.Web.Utilities.Wrappers;

namespace Academy.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (this.Environment.IsDevelopment())
            {
                services.AddDbContext<AcademySiteContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            }
            else
            {
                services.AddDbContext<AcademySiteContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AcademySiteContext>()
                .AddDefaultTokenProviders();



            if (this.Environment.IsDevelopment())
            {
                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
                    options.Lockout.MaxFailedAccessAttempts = 999;
                });
            }
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IExporter, PdfExporter>();
            services.AddScoped<IUserWrapper, UserWrapper>();

            services.AddMemoryCache();

            services.AddMvc()
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddKendo();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (this.Environment.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.ImportantExceptionHandling();
                
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();

            app.UseAuthentication();

            //app.UseKendo(env);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "notfound",
                    template: "{*url}",
                    defaults: new { controller = "Error", action = "PageNotFound" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
