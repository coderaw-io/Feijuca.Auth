using Feijuca.Auth.Application.Mappers;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Realm
{
    public class AddRealmsCommandHandler(IRealmService realmService) : IRequestHandler<AddRealmsCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddRealmsCommand request, CancellationToken cancellationToken)
        {
            foreach (var realmRequest in request.AddRealmsRequest)
            {
                var result = await realmService.AddNewRealmAsync(realmRequest.ToRealmEntity(), cancellationToken);
                if (!result.IsSuccess)
                {
                    return Result<bool>.Failure(RealmErrors.CreateRealmError);
                }
            }

            return Result<bool>.Success(true);
        }
    }
}
