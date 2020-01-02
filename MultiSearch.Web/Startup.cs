using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiSearch.DataAccess;
using MultiSearch.Domain.Contracts;
using MultiSearch.Engines;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MultiSearch.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<WorkDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("MultiSearchDB")));

            var builder = new ContainerBuilder();
            
            builder.RegisterType<WebPageService>().As<IWebPageService>()
                .SingleInstance();
            builder.Register(ctx => new HttpClient())
                .InstancePerDependency();

            builder.RegisterType<GoogleEngineHtml>()
                .Keyed<ISearchEngine>(MultiEngineProfile.Html)
                .Keyed<ISearchEngine>(MultiEngineProfile.Custom)
                .SingleInstance();
            builder.RegisterType<BingEngineHtml>()
                .Keyed<ISearchEngine>(MultiEngineProfile.Html)
                .SingleInstance();
            builder.RegisterType<YandexEngineHtml>()
                .Keyed<ISearchEngine>(MultiEngineProfile.Html)
                .SingleInstance();

            //Api versions of engines requiring setting up configuration
            var enginesSettingsSection = Configuration.GetSection("EnginesApiSettings");
            var enginesSettings = enginesSettingsSection.Get<EnginesApiSettings>();
            
            builder.RegisterType<GoogleEngineApi>()
                .Keyed<ISearchEngine>(MultiEngineProfile.Api)
                .WithParameter("apiKey", enginesSettings.GoogleKey)
                .WithParameter("searchEngineId", enginesSettings.GoogleSearchEngineId)
                .SingleInstance();
            builder.RegisterType<BingEngineApi>()
                .Keyed<ISearchEngine>(MultiEngineProfile.Api)
                .WithParameter("apiKey", enginesSettings.BingKey)
                .SingleInstance();
            builder.RegisterType<YandexEngineApi>()
                .Keyed<ISearchEngine>(MultiEngineProfile.Api)
                .WithParameter("apiUser", enginesSettings.YandexUser)
                .WithParameter("apiKey", enginesSettings.YandexKey)
                .SingleInstance();

            builder.RegisterType<MultiEngine>().As<ISearchEngine>()
                .WithParameter(
                    (p, c) => true,
                    (p, c) => c.ResolveKeyed<IEnumerable<ISearchEngine>>(MultiEngineProfile.Html)
                    )
                .SingleInstance();
            
            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
