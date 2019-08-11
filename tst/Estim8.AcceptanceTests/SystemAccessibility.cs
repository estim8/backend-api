using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Estim8.AcceptanceTests
{
    public class SystemAccessibility
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SystemAccessibility(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Should_Respond_Healthy()
        {
            //Arrange
            var baseUrl = Environment.GetEnvironmentVariable("ApiEndpoint");
            var url = new Uri(new Uri(baseUrl), "health");
            using (var client = new HttpClient())
            {
                
                //Act
                var response = await client.GetAsync(url);
                var responseBody = await response.Content.ReadAsStringAsync();

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("Healthy", responseBody);
            }
        }
    }
}