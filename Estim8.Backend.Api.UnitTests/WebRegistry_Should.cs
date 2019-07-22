using System.Threading.Tasks;
using AutoFixture;
using Estim8.Backend.Api.Registry;
using Estim8.TestInfrastructure;
using Lamar;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Estim8.Backend.Api.UnitTests
{
    public class WebRegistry_Should
    {
        [Theory(Skip = "Skip this for now, as I'll have to find a way to also inject the dependencies registered in Startup"), AutoMoqData]
        public async Task Construct_All_Dependencies(Fixture fixture)
        {
            //Arrange
            var sut = await Container.BuildAsync(c =>
            {
                c.For<ILoggerFactory>().Add((ILoggerFactory)null);
                c.IncludeRegistry<WebRegistry>();
            });
            
            //Act
            sut.AssertConfigurationIsValid(AssertMode.Full);

            //Assert
        }
    }
}