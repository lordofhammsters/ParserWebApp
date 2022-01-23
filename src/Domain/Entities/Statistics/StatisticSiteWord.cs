namespace ParserWebApp.Domain.Entities.Statistics;

public class StatisticSiteWord
{
    public int Id { get; set; }
    
    public int StatisticSiteId { get; set; }
    
    public string Word { get; set; } = null!;

    public int Count { get; set; }
    
    public StatisticSite Site { get; set; } = null!;
}