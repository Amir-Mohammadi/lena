using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffRequestMilestoneDetailSummaryMap : IEntityTypeConfiguration<StuffRequestMilestoneDetailSummary>
  {
    public void Configure(EntityTypeBuilder<StuffRequestMilestoneDetailSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffRequestMilestoneDetailSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderedQty);
      builder.Property(x => x.CargoedQty);
      builder.Property(x => x.ReciptedQty);
      builder.Property(x => x.StuffRequestMilestoneDetailId);
      builder.Property(x => x.QualityControlPassedQty);
      builder.HasRowVersion();
    }
  }
}