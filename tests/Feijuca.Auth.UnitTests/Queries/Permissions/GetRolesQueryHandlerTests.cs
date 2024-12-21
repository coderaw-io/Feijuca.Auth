using AutoFixture;
using Feijuca.Auth.Application.Queries.Permissions;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Feijuca.Auth.Api.UnitTests.Queries.Permissions
{
    public class GetRolesQueryHandlerTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly Mock<IClientRepository> _clientRepositoryMock = new();
        private readonly Mock<IClientRoleRepository> _roleRepositoryMock = new();
        private readonly GetRolesQueryHandler _handler;

        public GetRolesQueryHandlerTests()
        {
            _handler = new GetRolesQueryHandler(_clientRepositoryMock.Object, _roleRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_NoClientsExist_WhenGetClientsCalled_Then_ReturnsFailure()
        {
            // Arrange
            var rolesQuery = _fixture.Create<GetRolesQuery>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var rolesResult = Result<IEnumerable<ClientEntity>>.Failure(RoleErrors.GetRoleErrors);

            _clientRepositoryMock
                .Setup(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rolesResult);

            // Act
            var result = await _handler.Handle(rolesQuery, cancellationToken);

            // Assert
            result
                .Error
                .Should()
                .Be(RoleErrors.GetRoleErrors);

            _clientRepositoryMock.Verify(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()), Times.Once());
            _clientRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Given_ClientsExistButNoRoles_WhenGetRolesForClientCalled_Then_ReturnsEmptyResponse()
        {
            // Arrange
            var rolesQuery = _fixture.Create<GetRolesQuery>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var clients = _fixture.CreateMany<ClientEntity>();
            var clientsResult = Result<IEnumerable<ClientEntity>>.Success(clients);
            var rolesResult = Result<IEnumerable<Role>>.Failure(RoleErrors.GetRoleErrors);

            _clientRepositoryMock
                .Setup(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientsResult);

            _roleRepositoryMock
                .Setup(repo => repo.GetRolesForClientAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rolesResult);

            // Act
            var result = await _handler.Handle(rolesQuery, cancellationToken);

            // Assert
            result
                .IsSuccess
                .Should()
                .BeTrue();

            result
                .Response
                .Should()
                .BeEmpty(); 

            _clientRepositoryMock.Verify(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()), Times.Once);
            _clientRepositoryMock.VerifyNoOtherCalls();

            _roleRepositoryMock.Verify(repo => repo.GetRolesForClientAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(clients.Count()));
            _roleRepositoryMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task Given_ClientsAndRolesExist_WhenGetRolesForClientCalled_Then_ReturnsSuccess()
        {
            // Arrange
            var rolesQuery = _fixture.Create<GetRolesQuery>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var clients = _fixture.CreateMany<ClientEntity>();
            var clientsResult = Result<IEnumerable<ClientEntity>>.Success(clients);
            var roles = _fixture.CreateMany<Role>();
            var rolesResult = Result<IEnumerable<Role>>.Success(roles);

            _clientRepositoryMock
                .Setup(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientsResult);

            _roleRepositoryMock
                .Setup(repo => repo.GetRolesForClientAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rolesResult);

            // Act
            var result = await _handler.Handle(rolesQuery, cancellationToken);

            // Assert
            result
                .IsSuccess
                .Should()
                .BeTrue();

            _clientRepositoryMock.Verify(repo => repo.GetClientsAsync(It.IsAny<CancellationToken>()), Times.Once());
            _clientRepositoryMock.VerifyNoOtherCalls();

            _roleRepositoryMock.Verify(repo => repo.GetRolesForClientAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(roles.Count()));
            _roleRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
