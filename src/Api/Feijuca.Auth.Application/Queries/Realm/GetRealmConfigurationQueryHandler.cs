using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Queries.Realm
{
    public class GetRealmConfigurationQueryHandler(IRealmRepository _realmRepository) : IRequestHandler<GetRealmConfigurationQuery, string>
    {
        public async Task<string> Handle(GetRealmConfigurationQuery request, CancellationToken cancellationToken)
        {
            await _realmRepository.GetRealmConfigAsync(request.Name, cancellationToken);
            throw new NotImplementedException();
        }
    }
}
