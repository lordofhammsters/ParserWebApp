using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Statistics.Commands.ParseUrl;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Application.UnitTests.Statistics.Commands;

public class ParseUrlCommandTests : CommandTestBase
{
    [Test]
    public async Task ParseUrlCommandTest()
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("РАЗРАБОТКА ПРОГРАММНОГО ОБЕСПЕЧЕНИЯ"),
            });
        
        var client = new HttpClient(mockHttpMessageHandler.Object);
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<ParseUrlCommand.ParseUrlCommandHandler>>();
        
        var sut = new ParseUrlCommand.ParseUrlCommandHandler(httpClientFactoryMock.Object, loggerMock.Object, mediatorMock.Object, Context);
        var url = "https://www.simbirsoft.com/";

        // Act
        var result = await sut.Handle(new ParseUrlCommand { Url = url }, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(3, result.Items.Count);
        Assert.AreEqual(true, result.Items.Any(x => x.Word.Equals("РАЗРАБОТКА", StringComparison.OrdinalIgnoreCase)));
    }
}