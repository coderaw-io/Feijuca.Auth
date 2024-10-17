using Feijuca.Auth.Domain.Interfaces;
using Feijuca.Auth.Models;
using MongoDB.Driver;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class ConfigRepository(IMongoDatabase mongoDb) : IConfigRepository
    {
        private readonly IMongoCollection<AuthSettings> _collection = mongoDb.GetCollection<AuthSettings>("Configs");

        public async Task<bool> AddConfigAsync(AuthSettings newConfig)
        {
            await _collection.InsertOneAsync(newConfig);

            return true;
        }

        public AuthSettings GetConfig()
        {
            var config = _collection.Find(Builders<AuthSettings>.Filter.Empty).FirstOrDefault();

            return config;
        }
    }
}
