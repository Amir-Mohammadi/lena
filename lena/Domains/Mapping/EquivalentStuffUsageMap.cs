using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EquivalentStuffUsageMap : IEntityTypeConfiguration<EquivalentStuffUsage>
  {
    public void Configure(EntityTypeBuilder<EquivalentStuffUsage> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_EquivalentStuffUsage");
      builder.Property(x => x.Id);
      builder.Property(x => x.EquivalentStuffId);
      builder.Property(x => x.UsageQty);
      builder.Property(x => x.Status);
      builder.Property(x => x.ProductionPlanDetailId);
      builder.Property(x => x.ProductionOrderId);
      builder.HasOne(x => x.EquivalentStuff).WithMany(x => x.EquivalentStuffUsages).HasForeignKey(x => x.EquivalentStuffId);
      builder.HasOne(x => x.ProductionPlanDetail).WithMany(x => x.EquivalentStuffUsages).HasForeignKey(x => x.ProductionPlanDetailId);
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.EquivalentStuffUsages).HasForeignKey(x => x.ProductionOrderId);
    }
  }
}
