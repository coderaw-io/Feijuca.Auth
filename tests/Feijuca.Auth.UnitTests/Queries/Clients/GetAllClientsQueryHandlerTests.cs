using AutoFixture;
using Feijuca.Auth.Application.Queries.Clients;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Feijuca.Auth.Api.UnitTests.Queries.Clients
{
    public class GetAllClientsQueryHandlerTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly GetAllClientsQueryHandler _handler;

        public GetAllClientsQueryHandlerTests()
        {
            _handler = new GetAllClientsQueryHandler(_clientRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_ClientsExist_When_HandleIsCalled_Then_ShouldReturnSuccess()
        {
            // Arange
            var clientsQuery = _fixture.Create<GetAllClientsQuery>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var clients = _fixture.CreateMany<ClientEntity>();
            var clientsResult = Result<IEnumerable<ClientEntity>>.Success(clients);

            _clientRepositoryMock
                .Setup(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientsResult);

            // Act
            var result = await _handler.Handle(clientsQuery, cancellationToken);

            // Assert
            result
                .IsSuccess
                .Should()
                .BeTrue();

            _clientRepositoryMock.Verify(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()), Times.Once());
            _clientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Given_ClientsInvalid_When_HandleIsCalled_Then_ShouldReturnFailure()
        {
            // Arange
            var clientsQuery = _fixture.Create<GetAllClientsQuery>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var clientsResult = Result<IEnumerable<ClientEntity>>.Failure(ClientErrors.GetClientsErrors);

            _clientRepositoryMock
                .Setup(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientsResult);

            // Act
            var result = await _handler.Handle(clientsQuery, cancellationToken);

            // Assert
            result
                .Error
                .Should()
                .Be(ClientErrors.GetClientsErrors);

            _clientRepositoryMock.Verify(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()), Times.Once());
            _clientRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
