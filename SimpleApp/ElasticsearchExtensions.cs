using System;
using BooksApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

public static class ElasticsearchExtensions
{
    public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["elasticsearch:url"];
        var defaultIndex = configuration["elasticsearch:index"];
  
        var settings = new ConnectionSettings(new Uri(url))
            .DefaultIndex(defaultIndex);
  
        AddDefaultMappings(settings);
  
        var client = new ElasticClient(settings);
  
        services.AddSingleton(client);
  
        CreateIndex(client, defaultIndex);
    }
  
    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings.
            DefaultMappingFor<Book>(m => m
                .Ignore(p => p.Price)
            );
    }
  
    private static void CreateIndex(IElasticClient client, string indexName)
    {
        var createIndexResponse = client.Indices.Create(indexName,
            index => index.Map<Book>(x => x.AutoMap())
        );
    }
}