using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionLineProductionStepMap : IEntityTypeConfiguration<ProductionLineProductionStep>
  {
    public void Configure(EntityTypeBuilder<ProductionLineProductionStep> builder)
    {
      builder.HasKey(x => new
      {
        x.ProductionLineId,
        x.ProductionStepId
      });
      builder.ToTable("ProductionLineProductionSteps");
      builder.Property(x => x.ProductionLineId);
      builder.Property(x => x.ProductionStepId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionLine).WithMany(x => x.ProductionLineProductionSteps).HasForeignKey(x => x.ProductionLineId);
      builder.HasOne(x => x.ProductionStep).WithMany(x => x.ProductionLineProductionSteps).HasForeignKey(x => x.ProductionStepId);
    }
  }
}
