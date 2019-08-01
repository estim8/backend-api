using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Estim8.Backend.Api.Registry;
using Estim8.TestInfrastructure;
using Lamar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Estim8.Backend.Api.UnitTests
{
    public class WebRegistry_Should
    {
        [Theory(Skip = "Skip until a god way of injecting Startup registrations emerges"), AutoMoqData]
        public async Task Construct_All_Dependencies([Frozen]Mock<IConfiguration> configMock, Fixture fixture)
        {
            //Arrange
            var sut = await Container.BuildAsync(c =>
            {
                c.For<ILoggerFactory>().Use<NullLoggerFactory>();
                c.For(typeof(ILogger<>)).Use(typeof(NullLogger<>));
                c.For<IConfiguration>().Use(configMock.Object);
                c.IncludeRegistry<WebRegistry>();
            });
            
            //Act
            sut.AssertConfigurationIsValid(AssertMode.Full);

            //Assert
        }
    }
}