using Domain.Entities.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StatisticSiteConfiguration : IEntityTypeConfiguration<StatisticSite>
{
    public void Configure(EntityTypeBuilder<StatisticSite> builder)
    {
        builder.Property(t => t.Url)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(t => t.Created)
            .HasColumnType("datetime");

        builder.ToTable("StatisticSite");
    }
}