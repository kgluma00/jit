using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using JIT.Business.DI;
using JIT.Business.Interfaces;
using JIT.Business.Services;
using JIT.Core.Interfaces;
using JIT.MVC.Helpers;
using JIT.MVC.Mappings;
using JIT.Repository;
using JIT.Repository.DI;
using JIT.Repository.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace JIT.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "LoginCookie";
                    config.LoginPath = "/Home/Authenticate";
                });

            services.AddControllersWithViews();
            services.AddSingleton<AuthenticateUser>();
            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddDbContext<JitContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("JITConnectionString")));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new JitRepositoryMappingProfile());
                cfg.AddProfile(new JitClientMappingProfile());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddRepositoriesCollection();
            services.AddBusinessServicesCollection();
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

            //who are you?
            app.UseAuthentication();

            //are you allowed?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}
