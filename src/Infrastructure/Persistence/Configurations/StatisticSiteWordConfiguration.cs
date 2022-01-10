using Domain.Entities.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StatisticSiteWordConfiguration : IEntityTypeConfiguration<StatisticSiteWord>
{
    public void Configure(EntityTypeBuilder<StatisticSiteWord> builder)
    {
        builder.Property(t => t.Word)
            .HasColumnType("nvarchar(max)")
            .IsRequired();
        
        builder.ToTable("StatisticSiteWord");
    }
}