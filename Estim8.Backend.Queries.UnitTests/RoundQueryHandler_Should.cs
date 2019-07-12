using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Estim8.Backend.Persistence.Repositories;
using Estim8.Backend.Queries.Handlers;
using Estim8.Backend.Queries.Model;
using Estim8.Backend.Queries.Queries;
using Estim8.TestInfrastructure;
using Moq;
using Shouldly;
using Xunit;

namespace Estim8.Backend.Queries.UnitTests
{
    public class RoundQueryHandler_Should
    {
        [Theory, AutoMoqData]
        public async Task Get_A_Round(GetRoundById query, [Frozen] Mock<IRoundRepository> repoMock, RoundQueryHandler sut, Fixture fixture)
        {
            //Arrange
            var round = fixture.Build<Persistence.Model.Round>()
                .With(x => x.Id, query.RoundId)
                .Create();
            repoMock.Setup(x => x.GetById(query.GameId, query.RoundId)).ReturnsAsync(round);

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Round>();
            result.Id.ShouldBe(query.RoundId);
            result.GameId.ShouldBe(query.GameId);
        }
        
        [Theory, AutoMoqData]
        public async Task Return_Null_If_Round_Not_Found(GetRoundById query, [Frozen] Mock<IRoundRepository> repoMock, RoundQueryHandler sut, Fixture fixture)
        {
            //Arrange
            repoMock.Setup(x => x.GetById(query.GameId, query.RoundId))
                .ReturnsAsync((Persistence.Model.Round)null);

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.ShouldBeNull();
        }
        
        
        [Theory, AutoMoqData]
        public async Task Get_Current_Round(GetCurrentRound query, [Frozen] Mock<IRoundRepository> repoMock, RoundQueryHandler sut, Fixture fixture)
        {
            //Arrange
            var round = fixture.Build<Persistence.Model.Round>()
                .With(x => x.Id, query.GameId)
                .Create();
            repoMock.Setup(x => x.GetCurrentRound(query.GameId)).ReturnsAsync(round);

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<Round>();
            result.GameId.ShouldBe(query.GameId);
        }
        
        [Theory, AutoMoqData]
        public async Task Return_Null_If_No_Current_Round_Found(GetRoundById query, [Frozen] Mock<IRoundRepository> repoMock, RoundQueryHandler sut, Fixture fixture)
        {
            //Arrange
            repoMock.Setup(x => x.GetCurrentRound(query.GameId))
                .ReturnsAsync((Persistence.Model.Round)null);

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            result.ShouldBeNull();
        }
    }
}