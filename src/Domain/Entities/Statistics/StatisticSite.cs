namespace ParserWebApp.Domain.Entities.Statistics;

public class StatisticSite
{
    public StatisticSite()
    {
        SiteWords = new HashSet<StatisticSiteWord>();
    }
    
    public int Id { get; set; }
    
    public string? Url { get; set; }
    
    public DateTime Created { get; set; }
    
    public ICollection<StatisticSiteWord> SiteWords { get; private set; }
}