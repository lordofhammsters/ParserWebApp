namespace ParserWebApp.Application.Statistics.Queries.GetStatisticsByUrl;

public class StatisticsItems
{
    public List<StatisticsItemDto>? Items { get; set; }
}

public class StatisticsItemDto
{
    public string Word { get; set; } = null!;
    public int Count { get; set; }
}