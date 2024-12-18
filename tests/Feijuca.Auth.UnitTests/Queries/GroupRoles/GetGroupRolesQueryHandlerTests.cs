using AutoFixture;
using Feijuca.Auth.Application.Queries.GroupRoles;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Feijuca.Auth.Api.UnitTests.Queries.GroupRoles
{
    public class GetGroupRolesQueryHandlerTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly Mock<IGroupRolesRepository> _groupRolesMockRepository = new();
        private readonly GetGroupRolesQueryHandler _handler;

        public GetGroupRolesQueryHandlerTests()
        {
            _handler = new GetGroupRolesQueryHandler(_groupRolesMockRepository.Object);
        }

        [Fact]
        public async Task Given_ValidGroupId_WhenGroupRolesExist_Then_ReturnSuccess()
        {
            // Arrange
            var groupRolesQuery = _fixture.Create<GetGroupRolesQuery>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var clientMapping = _fixture.CreateMany<ClientMapping>();
            var groupRolesResult = Result<IEnumerable<ClientMapping>>.Success(clientMapping);

            _groupRolesMockRepository
                .Setup(repo => repo.GetGroupRolesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(groupRolesResult);

            // Act
            var result = await _handler.Handle(groupRolesQuery, cancellationToken);

            // Assert
            result
                .IsSuccess
                .Should()
                .BeTrue();

            _groupRolesMockRepository.Verify(repo => repo.GetGroupRolesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
            _groupRolesMockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Given_InvalidGroupId_WhenGetGroupRoleCalled_Then_ReturnFailure()
        {
            // Arrange
            var groupRolesQuery = _fixture.Create<GetGroupRolesQuery>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var groupRolesResult = Result<IEnumerable<ClientMapping>>.Failure(GroupErrors.GetUsersInGroupsError);

            _groupRolesMockRepository
                .Setup(repo => repo.GetGroupRolesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(groupRolesResult);

            // Act
            var result = await _handler.Handle(groupRolesQuery, cancellationToken);

            // Assert
            result
                .Error
                .Should()
                .Be(GroupErrors.GetUsersInGroupsError);

            _groupRolesMockRepository.Verify(repo => repo.GetGroupRolesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
            _groupRolesMockRepository.VerifyNoOtherCalls();
        }
    }
}
