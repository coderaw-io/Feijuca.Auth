using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Config
{
    public class AddConfigCommandHandler(IConfigRepository configRepository) : IRequestHandler<AddConfigCommand, Result<bool>>
    {
        public Task<Result<bool>> Handle(AddConfigCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
