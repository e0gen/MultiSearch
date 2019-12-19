using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiSearch.DataAccess;
using MultiSearch.Domain.Contracts;
using MultiSearch.Engines;

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
            services.AddDbContext<WorkDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("MultiSearchDB")));

            //DataAccess provider
            services.AddScoped<IWebPageService, WebPageService>();

            var enginesSettingsSection = Configuration.GetSection("EnginesApiSettings");
            var enginesSettings = enginesSettingsSection.Get<EnginesApiSettings>();

            //Api versions of engines requiring setting up configuration
            services.AddSingleton(_ =>
                new GoogleEngineApi(enginesSettings.GoogleKey, enginesSettings.GoogleSearchEngineId));
            services.AddSingleton(_ =>
                new BingEngineApi(enginesSettings.BingKey));
            services.AddSingleton(_ =>
                new YandexEngineApi(enginesSettings.YandexUser, enginesSettings.YandexKey));

            //Account free versions of engines
            services.AddSingleton<YandexEngineHtml>();
            services.AddSingleton<BingEngineHtml>();
            services.AddSingleton<GoogleEngineHtml>();

            //Multi engine. Inject the desired engines
            services.AddSingleton<ISearchEngine>(provider =>
                new MultiEngine(new ISearchEngine[] {
                    provider.GetRequiredService<YandexEngineHtml>(),
                    provider.GetRequiredService<BingEngineHtml>(),
                    provider.GetRequiredService<GoogleEngineHtml>(),
                    }));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
