using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Estim8.Backend.Persistence.Repositories;
using Estim8.Backend.Queries.Handlers;
using Estim8.Backend.Queries.Model;
using Estim8.TestInfrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Estim8.Backend.Queries.UnitTests
{
    public class GameQueryHandler_Should
    {
        [Theory, AutoMoqData]
        public async Task Get_A_Game(GetGameById query, [Frozen]Mock<IRepository<Persistence.Model.Game>> repoMock, Fixture fixture)
        {
            //Arrange
            var game = fixture.Build<Persistence.Model.Game>().With(x => x.Id, query.Id).Create();
            repoMock.Setup(x => x.GetById(query.Id)).ReturnsAsync(game);

            var sut = fixture.Create<GameQueryHandler>();
            
            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Game>();
            result.Id.ShouldBe(query.Id);
        }
        
        [Theory, AutoMoqData]
        public async Task Return_Null_If_Game_Not_Found(GetGameById query, [Frozen]Mock<IRepository<Persistence.Model.Game>> repoMock, Fixture fixture)
        {
            //Arrange
            repoMock.Setup(x => x.GetById(query.Id)).ReturnsAsync((Persistence.Model.Game)null);

            var sut = fixture.Create<GameQueryHandler>();
            
            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.ShouldBeNull();
        }
    }
}