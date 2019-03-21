using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PAIS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PAIS
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:PAISBooks:ConnectionString"]));
            services.AddTransient<IBookRepository, EFBookRepository>();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes => {
            routes.MapRoute(
                name: "pagination",
                template: "Books/Page{bookPage}",
                defaults: new { Controller = "Book", action = "List" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Book}/{action=List}/{id?}");
            });
            SeedData.EnsurePopulated(app);
        }
    }
}
