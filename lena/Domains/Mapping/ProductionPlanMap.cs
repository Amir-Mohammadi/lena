using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionPlanMap : IEntityTypeConfiguration<ProductionPlan>
  {
    public void Configure(EntityTypeBuilder<ProductionPlan> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ProductionPlan");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionRequestId);
      builder.Property(x => x.EstimatedDate).HasColumnType("smalldatetime");
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Status);
      builder.Property(x => x.IsTemporary);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.BillOfMaterialStuffId);
      builder.HasOne(x => x.ProductionRequest).WithMany(x => x.ProductionPlans).HasForeignKey(x => x.ProductionRequestId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ProductionPlans).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.ProductionPlanSummary).WithOne(x => x.ProductionPlan).HasForeignKey<ProductionPlanSummary>(x => x.ProductionPlanId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.ProductionPlans).HasForeignKey(x => new { x.BillOfMaterialVersion, x.BillOfMaterialStuffId });
    }
  }
}