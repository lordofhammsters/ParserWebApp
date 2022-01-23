using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ParserWebApp.Application.Common.Interfaces;
using ParserWebApp.Domain.Entities.Statistics;

namespace ParserWebApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<StatisticSite> StatisticSites { get; set; }
    
    public DbSet<StatisticSiteWord> StatisticSiteWords { get; set; }
    
    
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}