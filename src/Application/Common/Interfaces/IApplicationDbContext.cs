using Microsoft.EntityFrameworkCore;
using ParserWebApp.Domain.Entities.Statistics;

namespace ParserWebApp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<StatisticSite> StatisticSites { get; set; }
    
    DbSet<StatisticSiteWord> StatisticSiteWords { get; set; }
    
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}