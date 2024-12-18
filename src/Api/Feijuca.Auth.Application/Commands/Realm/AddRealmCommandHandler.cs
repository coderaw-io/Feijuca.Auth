using Feijuca.Auth.Application.Mappers;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Realm
{
    public class AddRealmCommandHandler(IRealmService realmService) : IRequestHandler<AddRealmCommand, bool>
    {
        public async Task<bool> Handle(AddRealmCommand request, CancellationToken cancellationToken)
        {
            var result = await realmService.AddNewRealmAsync(request.AddRealmRequest.ToRealmEntity(), cancellationToken);
            if (result.IsSuccess)
            {
                return true;
            }

            return false;
        }
    }
}
