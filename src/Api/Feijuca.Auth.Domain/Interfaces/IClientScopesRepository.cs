﻿using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IClientScopesRepository : IBaseRepository
    {
        Task<bool> AddClientScopesAsync(ClientScopesEntity clientScopesEntity, CancellationToken cancellationToken);
        Task<bool> AddClientScopeToClientAsync(string clientId, string clientScopeId, bool isOptional, CancellationToken cancellationToken);
        Task<IEnumerable<ClientScopeEntity>> GetClientScopesAsync(CancellationToken cancellationToken);
    }
}