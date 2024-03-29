using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KernelCars.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using KernelCars.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

//using System.Configuration;

namespace KernelCars
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();
            //services.AddMvc(options =>
            //{
            //    options.EnableEndpointRouting = false;
            //});
            //services.AddMvc().AddNewtonsoftJson(opt => opt.SerializerSettings.MaxDepth=64);//.AddNewtonsoftJson();//.AddJsonOptions(options => {
            //options.JsonSerializerOptions.MaxDepth = 256;  // or however deep you need
            //});

            //services.AddMvc().AddNewtonsoftJson(options =>
            //    { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //        options.SerializerSettings.MaxDepth = 64;
            //    });//.AddJsonOptions(opt => opt.JsonSerializerOptions.MaxDepth = 1);

            //services.AddControllers().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //.AddJsonOptions(opt=>opt.JsonSerializerOptions.MaxDepth=255);//.AddNewtonsoftJson();

            //services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.MaxDepth = 255);
            //services.AddControllersWithViews().AddJsonOptions(opt => opt.JsonSerializerOptions.MaxDepth = 255);
            //services.AddRazorPages().AddJsonOptions(opt => opt.JsonSerializerOptions.MaxDepth = 255);
            //            services.AddControllers().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //);

            //            services.AddControllersWithViews()
            //    .AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //);


            //services.AddServerSideBlazor();

            //services.AddScoped(sp => new HttpClient
            //{
            //    BaseAddress = new Uri("http://localhost")
            //});
            services.AddSingleton<KernelCars.Components.ReportingService>();
            services.AddScoped(sp => new HttpClient());

            services.AddScoped<UserInfo>();

            services.AddDbContext<DataContext>(options=>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(options =>
                //options.UseSqlServer(Configuration["KernelCarsIdentity:ConnectionString"]));
                options.UseSqlServer(Configuration.GetConnectionString("KernelCarsIdentity")));

            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

          
            //services.AddIdentity<IdentityUser, IdentityRole>()
            //.AddEntityFrameworkStores<AppIdentityDbContext>();
            //.AddDefaultTokenProviders();

            //services.AddMvc();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            
            app.UseRouting();

            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseAuthorization();
            //app.UseMvcWithDefaultRoute();


            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "pagination",
                //    pattern: "Cars/Page{carPage}",
                //    defaults: new { Controller = "Car", action = "Index" });

                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Car}/{action=List}/{id?}");// .MapControllers();

                endpoints.MapControllerRoute("default",
                    "/{controller=Car}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute("controllers",
                //    "controllers/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                //endpoints.MapFallbackToController("Blazor", "Home");
            });

            IdentitySeedData.CreateAdminAccount(app.ApplicationServices, Configuration);

            //app.UseAuthentication();


            //IdentitySeedData.EnsurePopulated(app);

            //app.UseMvcWithDefaultRoute();
            //app.UseMvc(routes => {
            //    routes.MapRoute(
            //        name: "pagination",
            //        template: "Cars/Page{carPage}",
            //        defaults: new { Controller = "Car", action = "Index" });

            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Car}/{action=Index}/{id?}");
            //});
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action}",
            //        defaults: new { controller = "Car", action = "Index" });
            //});
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World! From Kernel Cars");
            //    });
            //});
        }
    }
}
