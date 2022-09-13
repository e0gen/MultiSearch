using Autofac.Extensions.DependencyInjection;
using Autofac;
using MultiSearch.Infrastructure;
using MultiSearch.Domain.Contracts;
using MultiSearch.Engines;
using MultiSearch.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WorkDbContext>(options =>
    options.UseInMemoryDatabase(databaseName: "MultiSearchDB")
    //options.UseSqlServer(builder.Configuration.GetConnectionString("MultiSearchDB"))
);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((ctx, builder) => {
    
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
    var enginesSettingsSection = ctx.Configuration.GetSection("EnginesApiSettings");
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
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Search}/{action=Search}/{id?}");

app.MapControllerRoute(
    name: "history",
    pattern: "{controller=History}/{action=Index}/{id?}");

app.Run();