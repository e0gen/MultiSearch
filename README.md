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
To turn on the api version, configure the keys in the configuration file and set multi engine resolving profile to Api
in composition root (Statup.cs)

```
(p, c) => c.ResolveKeyed<IEnumerable<ISearchEngine>>(MultiEngineProfile.Html)
```
with
```
(p, c) => c.ResolveKeyed<IEnumerable<ISearchEngine>>(MultiEngineProfile.Api)
```

### Engine expanding

List of supported search engines can easy expanded with additional implementation of ISearchEngine interface.

Register it in composition root and specify multi engine profile in which it have to belong:
```
builder.RegisterType<CustomEngine>()
                .Keyed<ISearchEngine>(MultiEngineProfile.Html)
                .SingleInstance();
```

## Running the tests

Unit tests for search engine API implementation also requring keys in the separate configuration file **testsettings.json**. 

## Built on
* C# 7
* .NET Core 2.2
* ASP.NET Core MVC
* Entity Framework Core
* MS SQL Server

## Extra
* [Skeleton](http://getskeleton.com/) - CSS style

## Authors

* **Evgeny Nevzorov** - [e0gen](https://github.com/e0gen)
