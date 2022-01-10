using Domain.Entities.Statistics;

namespace Application.Statistics.Queries.GetStatisticsByUrl;

public class StatisticsItems
{
    public List<StatisticsItemDto> Items { get; set; }
}

public class StatisticsItemDto
{
    public string Word { get; set; }
    public int Count { get; set; }
}