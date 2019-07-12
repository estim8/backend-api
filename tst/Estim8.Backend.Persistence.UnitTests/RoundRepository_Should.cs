using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using CachingFramework.Redis.Contracts;
using CachingFramework.Redis.Contracts.Providers;
using CachingFramework.Redis.Contracts.RedisObjects;
using Castle.Core.Internal;
using Estim8.Backend.Persistence.Model;
using Estim8.Backend.Persistence.Repositories;
using Estim8.TestInfrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Estim8.Backend.Persistence.UnitTests
{
    public class RoundRepository_Should
    {
        [Theory, AutoMoqData]
        public async Task Add_A_Round(Guid gameId, Round round, [Frozen] Mock<IContext> ctxMock, Fixture fixture)
        {
            //Arrange
            var listMock = SetupListMock(ctxMock, It.IsAny<string>(), new List<Round>(),  fixture);
            
            var sut = fixture.Create<RoundRepository>();

            //Act
            await sut.AddRound(gameId, round);

            //Assert
            listMock.Verify(x => x.AddAsync(round), Times.Once);
        }

        [Theory, AutoMoqData]
        public async Task Get_A_Round(Guid gameId, Guid roundId, [Frozen] Mock<IContext> ctxMock, Fixture fixture)
        {
            //Arrange
            var hashKey = $"game:id:{gameId}:rounds";
            var db = new List<Round> {new Round{Id = roundId}};
            var listMock = SetupListMock(ctxMock, hashKey, db, fixture);
            
            var sut = fixture.Create<RoundRepository>();

            //Act
            var result = await sut.GetById(gameId, roundId);

            //Assert
            result.ShouldNotBeNull();
        }

        [Theory, AutoMoqData]
        public async Task Get_Current_Round(Guid gameId, [Frozen] Mock<IContext> ctxMock, Fixture fixture)
        {
            //Arrange
            var hashKey = $"game:id:{gameId}:rounds";
            var db = fixture.CreateMany<Round>(10).ToList();
            var listMock = SetupListMock(ctxMock, hashKey, db, fixture);
            
            var sut = fixture.Create<RoundRepository>();

            //Act
            var result = await sut.GetCurrentRound(gameId);

            //Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(db.Last().Id);
            
        }
        
        private Mock<IRedisList<T>> SetupListMock<T>(Mock<IContext> ctxMock, string hashKey, List<T> list, IFixture fixture)
        {
            var collProviderMock = fixture.Freeze<Mock<ICollectionProvider>>();
            var listMock = fixture.Freeze<Mock<IRedisList<T>>>();
            
            listMock.Setup(x => x.GetRangeAsync(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync((long start, long stop) =>
            {
                var startIdx = (int) (start >= 0 ? start : list.Count + start);
                var stopIdx = (int) (stop >= 0 ? stop : list.Count + stop);

                return list.Skip(startIdx).Take(stopIdx - startIdx + 1);
            });
            
            ctxMock.SetupGet(x => x.Collections).Returns(collProviderMock.Object);
            collProviderMock.Setup(x => x.GetRedisList<T>(It.Is<string>(p => hashKey == It.IsAny<string>() || p == hashKey))).Returns(listMock.Object);
            
            return listMock;
        }
    }
}