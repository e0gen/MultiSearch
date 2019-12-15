﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiSearch.DataAccess;
using MultiSearch.Domain;
using MultiSearch.Domain.Contracts;
using SearchEngines;

namespace MultiSearch.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<WorkDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("RazorPagesMovieContext")));
            services.AddDbContext<WorkDbContext>(options =>
                options.UseInMemoryDatabase("VirtualDb"));

            services.AddScoped<IItemService, ItemService>();
            //services.AddScoped<ISearch, GoogleSearch>();

            //services.AddSingleton<ISearch, GoogleSearch>(_ =>
            //{
            //    return new GoogleSearch("AIzaSyAt8AkrmkiLVghrcKA3lFh37R79rSG0NsE", "003470263288780838160:ty47piyybua");
            //});
            services.AddSingleton<ISearch, BingSearch>(_ =>
            {
                return new BingSearch("4202bcd3d7c546debedbc8f308def029");
            });
            //services.AddSingleton<ISearch, YandexSearch>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc();
            //    routes =>
            //{
            //    routes.MapRoute(
            //        name: "history",
            //        template: "{controller=History}/{action=Index}/{id?}");
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Search}/{action=Index}");
            //});
        }
    }
}