using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;
using Estim8.TestInfrastructure;
using Moq;
using Shouldly;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Xunit;

namespace Estim8.Backend.Persistence.UnitTests
{
    public class GameRepository_Should
    {
        [Fact]
        public async Task Delete_A_Game()
        {
            //Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var gameId = fixture.Create<Guid>();
            var cacheMock = fixture.Freeze<Mock<IRedisCacheClient>>();
            var dbMock = fixture.Freeze<Mock<IRedisDatabase>>();
            dbMock.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);
            cacheMock.Setup(x => x.GetDbFromConfiguration()).Returns(dbMock.Object);

            var sut = fixture.Create<GameRepository>();
            
            //Act
            var answer = await sut.Delete(gameId);

            //Assert
            answer.ShouldBe(true, "Redis should acknowledge the delete");
            dbMock.Verify(x => x.RemoveAsync(gameId.ToString(), CommandFlags.DemandMaster), Times.Once, "All Redis write operations must be done with DemandMaster flag");
        }

        [Theory(Skip = "Skipped until Rider/ReSharper fix for AutoData attributes is released"), AutoMoqData]
        public async Task Create_A_Game(Game game)
        {
            //Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var cacheMock = fixture.Freeze<Mock<IRedisCacheClient>>();
            var dbMock = fixture.Freeze<Mock<IRedisDatabase>>();
            dbMock.Setup(x => x.AddAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<When>(), It.IsAny<CommandFlags>())).ReturnsAsync(true);
            cacheMock.Setup(x => x.GetDbFromConfiguration()).Returns(dbMock.Object);

            var sut = fixture.Create<GameRepository>();
            
            //Act
            var answer = await sut.Upsert(game);

            //Assert
            answer.ShouldBe(true, "Redis should acknowledge the upsert");
            dbMock.Verify(x => x.AddAsync(game.Id.ToString(), It.IsAny<object>(), It.IsAny<When>(), CommandFlags.DemandMaster), Times.Once, "All Redis write operations must be done with DemandMaster flag");
        }
    }
}