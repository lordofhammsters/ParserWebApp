using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using ParserWebApp.Application.Statistics.Queries.GetStatisticsByUrl;
using ParserWebApp.Domain.Entities.Statistics;

namespace Application.UnitTests.Statistics.Queries;

public class GetStatisticsByUrlQueryTests : CommandTestBase
{
    private const string TestUrl = "https://www.test.com/";
    private const string TestWord = "разработка";
    
    [OneTimeSetUp]
    public async Task Init()
    {
        Context.StatisticSites.Add(new StatisticSite()
        {
            Url = TestUrl,
            Created = DateTime.Now,
            SiteWords =
            {
                new StatisticSiteWord() {Word = TestWord, Count = 5}
            }
        });
        await Context.SaveChangesAsync();
    }
    
    [Test]
    public async Task GetStatisticsByUrlTest()
    {
        var sut = new GetStatisticsByUrlQueryHandler(Context);

        // Act
        var result = await sut.Handle(new GetStatisticsByUrlQuery { Url = TestUrl }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().NotBeNull();
        result.Items!.Count.Should().Be(1);
        result.Items.Any(x => x.Word.Equals(TestWord, StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
    }
}