using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Statistics.Queries.GetStatisticsByUrl;
using Domain.Entities.Statistics;
using NUnit.Framework;

namespace Application.UnitTests.Statistics.Queries;

public class GetStatisticsByUrlQueryTests : CommandTestBase
{
    private const string TestUrl = "https://www.simbirsoft.com/";
    private const string TestWord = "разработка";
    
    [OneTimeSetUp]
    public async Task Init()
    {
        _context.StatisticSites.Add(new StatisticSite()
        {
            Url = TestUrl,
            Created = DateTime.Now,
            SiteWords =
            {
                new StatisticSiteWord() {Word = TestWord, Count = 5}
            }
        });
        await _context.SaveChangesAsync();
    }
    
    [Test]
    public async Task GetStatisticsByUrlTest()
    {
        var sut = new GetStatisticsByUrlQueryHandler(_context);

        // Act
        var result = await sut.Handle(new GetStatisticsByUrlQuery { Url = TestUrl }, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.AreEqual(1, result.Items.Count);
        Assert.AreEqual(true, result.Items.Any(x => x.Word.Equals(TestWord, StringComparison.OrdinalIgnoreCase)));
    }
}