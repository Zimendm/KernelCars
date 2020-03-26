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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

            services.AddMvc();//.AddJsonOptions(opt=>opt.JsonSerializerOptions.MaxDepth=255);//.AddNewtonsoftJson();

            //services.AddControllers().AddJsonOptions(opt => opt.JsonSerializerOptions.MaxDepth = 255);
            //services.AddControllersWithViews().AddJsonOptions(opt => opt.JsonSerializerOptions.MaxDepth = 255);
            //services.AddRazorPages().AddJsonOptions(opt => opt.JsonSerializerOptions.MaxDepth = 255);
            services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

            services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


            services.AddServerSideBlazor();

            services.AddDbContext<DataContext>(options=>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            

            //services.AddControllersWithViews();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "pagination",
                    pattern: "Cars/Page{carPage}",
                    defaults: new { Controller = "Car", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");// .MapControllers();
                //endpoints.MapRazorPages();
                endpoints.MapBlazorHub();

                endpoints.MapFallbackToController("Blazor", "Home");
            });


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
