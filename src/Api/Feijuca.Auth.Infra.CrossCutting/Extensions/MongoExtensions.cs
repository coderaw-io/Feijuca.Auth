using Feijuca.Auth.Infra.CrossCutting.Config;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Feijuca.Auth.Infra.CrossCutting.Extensions
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, MongoSettings mongoSettings)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            var clientSettings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionString);
            var mongoClient = new MongoClient(clientSettings);
            services.AddSingleton<IMongoClient>(_ => mongoClient);

            services.AddSingleton(sp =>
            {
                var mongoClient = sp.GetService<IMongoClient>();
                var db = mongoClient!.GetDatabase(mongoSettings.Database);

                return db;
            });


            return services;
        }
    }
}