using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TinyClothes.Data;


namespace TinyClothes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //private void ConfigDbContext (DbContextOptionsBuilder options)
        //{
        //    options.UseSqlServer("db connection goes here");
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("ClothesDB");

            services.AddControllersWithViews();

            services.AddDbContext<StoreContext>(options => options.UseSqlServer(connection)); //lambda syntax passes in an anonymous function as a parameter

            //same as above
            //services.AddDbContext<StoreContext>(ConfigDbContext);

            services.AddDistributedMemoryCache();

            //add and configure session
            services.AddSession(options =>
            {
                options.Cookie.Name = ".TinyClothes.session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true; //session cookie get created even if user doesn't accept cookie policy
            });
              
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession(); //allows session data to be accessed

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
