using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using CachingFramework.Redis.Contracts;
using CachingFramework.Redis.Contracts.Providers;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;
using Estim8.TestInfrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Estim8.Backend.Persistence.UnitTests
{
    public class GameRepository_Should
    {
        [Theory, AutoMoqData]
        public async Task Delete_A_Game(Guid gameId, [Frozen]Mock<IContext> redisCtxMock, [Frozen]Mock<ICacheProvider> redisCacheProviderMock, Fixture fixture)
        {
            //Arrange
            redisCtxMock.SetupGet(x => x.Cache).Returns(redisCacheProviderMock.Object);
            redisCacheProviderMock.Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            
            var sut = fixture.Create<GameRepository>();

            //Act
            var answer = await ((IGameRepository)sut).Delete(gameId);

            //Assert
            answer.ShouldBe(true, "Redis should acknowledge the delete");
            redisCacheProviderMock.Verify(x => x.RemoveAsync(sut.ToKey(gameId, null)), Times.Once,"Redis should remove entry from hashset");
        }

        [Theory, AutoMoqData]
        public async Task Create_A_Game(Game game, [Frozen]Mock<IContext> redisCtxMock, [Frozen]Mock<ICacheProvider> redisCacheProviderMock, Fixture fixture)
        {
            //Arrange
            redisCtxMock.SetupGet(x => x.Cache).Returns(redisCacheProviderMock.Object);
            redisCacheProviderMock.Setup(x => x.SetObjectAsync(It.IsAny<string>(), game, It.IsAny<TimeSpan>(), It.IsAny<When>()));

            var sut = fixture.Create<GameRepository>();
            
            //Act
            await ((IGameRepository)sut).Upsert(game);

            //Assert
            redisCacheProviderMock.Verify(x => x.SetObjectAsync(sut.ToKey(game.Id, null), game, It.IsAny<TimeSpan?>(), It.IsAny<When>()), Times.Once);
        }
    }
}