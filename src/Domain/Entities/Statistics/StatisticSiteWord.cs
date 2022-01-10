namespace Domain.Entities.Statistics;

public class StatisticSiteWord
{
    public int Id { get; set; }
    
    public int StatisticSiteId { get; set; }
    
    public string Word { get; set; }
    
    public int Count { get; set; }
    
    public StatisticSite Site { get; set; }
}