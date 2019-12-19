# MultiSearch Engine

Simple designed search engine application which encapsulate requests to popular search engines(Yandex, Google and Bing) and display result of the fastest one.
Application also keeps search history with filtration.

## Setup

Set the connection string to you database server in **appsettings.json**
Data access module uses code first aproach so to initilize data base run command in package manager console:
```
PM> update-database
```

### API versions

The project contains implementation of 3 search engines, each of which has 2 implementation options: through HTML parsing and with direct access to the search engine API.
By default, application resolves  HTML versions, which do not require additional configuration.
To turn on the api version, configure the keys in the configuration file and replace search engine resolving in composition root (Statup.cs)

```
services.AddSingleton<ISearchEngine>(provider =>
                new MultiEngine(new ISearchEngine[] {
                    ...
                    provider.GetRequiredService<BingEngineHtml>(),
                    }));
```
with
```
services.AddSingleton<ISearchEngine>(provider =>
                new MultiEngine(new ISearchEngine[] {
                    ...
                    provider.GetRequiredService<BingEngineApi>(),
                    }));
```

### Engine expanding

List of supported search engines can easy expanded with additional implementation of ISearchEngine interface.

Include it in composition root:
```
services.AddSingleton<CustomEngine>();
```
And inject to abstraction of speed competiotion
```
services.AddSingleton<ISearchEngine>(provider =>
                new MultiEngine(new ISearchEngine[] {
                    ...
                    provider.GetRequiredService<CustomEngine>(),
                    }));
```

## Running the tests

Unit tests for search engine API implementation also requring keys in the separate configuration file **testsettings.json**. 

## Built on
* C#
* .NET Core
* ASP.NET Core MVC
* Entity Framework Core
* MS SQL Server

## Extra
* [Skeleton](http://getskeleton.com/) - CSS style

## Authors

* **Evgeny Nevzorov** - [e0gen](https://github.com/e0gen)
