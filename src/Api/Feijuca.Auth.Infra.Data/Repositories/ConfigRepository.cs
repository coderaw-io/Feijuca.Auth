using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using MongoDB.Driver;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class ConfigRepository(IMongoDatabase mongoDb) : IConfigRepository
    {
        private readonly IMongoCollection<KeycloakSettingsEntity> _collection = mongoDb.GetCollection<KeycloakSettingsEntity>("Configs");

        public async Task<bool> AddConfigAsync(KeycloakSettingsEntity newConfig, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(newConfig, new InsertOneOptions(), cancellationToken);

            return true;
        }

        public async Task<KeycloakSettingsEntity> GetConfigAsync()
        {
            var config = await _collection.Find(Builders<KeycloakSettingsEntity>.Filter.Empty).FirstOrDefaultAsync();

            return config;
        }

        public async Task<bool> UpdateRealmConfigAsync(Guid id, KeycloakSettingsEntity keycloakSettings)
        {
            var filter = Builders<KeycloakSettingsEntity>.Filter.Eq(e => e.Id, id);

            var update = Builders<KeycloakSettingsEntity>.Update
                .Set(e => e.Realms, keycloakSettings.Realms);

            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}
