using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionPlanDetailMap : IEntityTypeConfiguration<ProductionPlanDetail>
  {
    public void Configure(EntityTypeBuilder<ProductionPlanDetail> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ProductionPlanDetail");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionPlanId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.ProductionPlanDetailLevelId);
      builder.Property(x => x.Status);
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.BillOfMaterialStuffId);
      builder.HasOne(x => x.ProductionPlan).WithMany(x => x.ProductionPlanDetails).HasForeignKey(x => x.ProductionPlanId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ProductionPlanDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.ProductionPlanDetailLevel).WithMany(x => x.ProductionPlanDetails).HasForeignKey(x => x.ProductionPlanDetailLevelId);
      builder.HasOne(x => x.ProductionPlanDetailSummary).WithOne(x => x.ProductionPlanDetail).HasForeignKey<ProductionPlanDetailSummary>(x => x.ProductionPlanDetailId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.ProductionPlanDetails).HasForeignKey(x => new { x.BillOfMaterialVersion, x.BillOfMaterialStuffId });
    }
  }
}
