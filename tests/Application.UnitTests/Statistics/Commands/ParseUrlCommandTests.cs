using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using ParserWebApp.Application.Statistics.Commands.ParseUrl;
using ParserWebApp.Application.Statistics.Queries.GetStatisticsByUrl;

namespace Application.UnitTests.Statistics.Commands;

public class ParseUrlCommandTests : CommandTestBase
{
    private const string TestSiteContent = "РАЗРАБОТКА ПРОГРАММНОГО ОБЕСПЕЧЕНИЯ";
    private const string TestWord = "разработка";
    
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
                Content = new StringContent(TestSiteContent)
            });
        
        var client = new HttpClient(mockHttpMessageHandler.Object);
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetStatisticsByUrlQuery>(), CancellationToken.None))
            .ReturnsAsync(new StatisticsItems() {Items = new List<StatisticsItemDto>()});
        
        var loggerMock = new Mock<ILogger<ParseUrlCommand.ParseUrlCommandHandler>>();
        
        var sut = new ParseUrlCommand.ParseUrlCommandHandler(httpClientFactoryMock.Object, loggerMock.Object, mediatorMock.Object, Context);
        var url = "https://test.com/";

        // Act
        var result = await sut.Handle(new ParseUrlCommand { Url = url }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items!.Count.Should().Be(3);
        result.Items.Any(x => x.Word.Equals(TestWord, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
    }
}