using AutoFixture;
using Feijuca.Auth.Application.Commands.Users;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Feijuca.Auth.Api.UnitTests.Command
{
    public class RevokeUserSessionsCommandHandlerTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly RevokeUserSessionsCommandHandler _handler;

        public RevokeUserSessionsCommandHandlerTests()
        {
            _handler = new RevokeUserSessionsCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_RevokeUserSessions_WhenResultIsSuccessful()
        {
            // Arrange
            var revokeRequest = _fixture.Create<RevokeUserSessionsCommand>();
            var cancellationToken = _fixture.Create<CancellationToken>();

            _userRepositoryMock
                .Setup(repo => repo.RevokeSessionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<bool>.Success(true));

            // Act
            var result = await _handler.Handle(revokeRequest, cancellationToken);

            // Assert
            result
                .IsSuccess
                .Should()
                .BeTrue();

            _userRepositoryMock.Verify(repo => repo.RevokeSessionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_Should_ReturnFailureResult_WhenRevokeSessionsReturnsError()
        {
            var revokeRequest = _fixture.Create<RevokeUserSessionsCommand>();
            var cancellationToken = _fixture.Create<CancellationToken>();
            var resultError = Result<bool>.Failure(UserErrors.RevokeSessionsError);

            _userRepositoryMock
                .Setup(repo => repo.RevokeSessionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultError);

            // Act
            var result = await _handler.Handle(revokeRequest, cancellationToken);

            // Assert
            result
                .Error
                .Should()
                .Be(UserErrors.RevokeSessionsError);

            _userRepositoryMock.Verify(repo => repo.RevokeSessionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
            _userRepositoryMock.VerifyNoOtherCalls();
        }
    }
}
