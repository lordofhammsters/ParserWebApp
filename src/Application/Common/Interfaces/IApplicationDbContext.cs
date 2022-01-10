using Domain.Entities.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<StatisticSite> StatisticSites { get; set; }
    
    DbSet<StatisticSiteWord> StatisticSiteWords { get; set; }
    
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}