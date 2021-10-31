using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffFractionResultMap : IEntityTypeConfiguration<StuffFractionResult>
  {
    public void Configure(EntityTypeBuilder<StuffFractionResult> builder)
    {
      builder.HasKey(x => x.StuffId);
      builder.ToTable("StuffFractionResults");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffName).IsRequired();
      builder.Property(x => x.StuffTitle).IsRequired();
      builder.Property(x => x.StuffNoun).IsRequired();
      builder.Property(x => x.StuffCode).IsRequired();
      builder.Property(x => x.StuffFaultyPercentage);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.UnitName).IsRequired();
      builder.Property(x => x.UnitTypeId);
      builder.Property(x => x.UnitTypeName).IsRequired();
      builder.Property(x => x.StuffStockSafety);
      builder.Property(x => x.AvailableAmount);
      builder.Property(x => x.BlockedAmount);
      builder.Property(x => x.PlanAmount);
      builder.Property(x => x.QualityControlAmount);
      builder.Property(x => x.WasteAmount);
      builder.Property(x => x.TotalAmount);
      builder.Property(x => x.RemainedAmount);
      builder.Property(x => x.BeforePeriodPlanPlusAmount);
      builder.Property(x => x.BeforePeriodPlanMinusAmount);
      builder.Property(x => x.BeforePeriodPlanAmount);
      builder.Property(x => x.PeriodPlanPlusAmount);
      builder.Property(x => x.PeriodPlanMinusAmount);
      builder.Property(x => x.PeriodPlanAmount);
      builder.Property(x => x.AfterPeriodPlanPlusAmount);
      builder.Property(x => x.AfterPeriodPlanMinusAmount);
      builder.Property(x => x.AfterPeriodPlanAmount);
      builder.Property(x => x.Status);
      builder.HasRowVersion();
      builder.Property(x => x.BeforePeriodPlanConsumeAmount);
      builder.Property(x => x.BeforePeriodPlanProduceAmount);
      builder.Property(x => x.BeforePeriodPlanSaleOrderAmount);
      builder.Property(x => x.BeforePeriodPlanPurchaseRequestAmount);
      builder.Property(x => x.BeforePeriodPlanPurchaseOrderAmount);
      builder.Property(x => x.BeforePeriodPlanCargoAmount);
      builder.Property(x => x.PeriodPlanConsumeAmount);
      builder.Property(x => x.PeriodPlanProduceAmount);
      builder.Property(x => x.PeriodPlanSaleOrderAmount);
      builder.Property(x => x.PeriodPlanPurchaseRequestAmount);
      builder.Property(x => x.PeriodPlanPurchaseOrderAmount);
      builder.Property(x => x.PeriodPlanCargoAmount);
      builder.Property(x => x.AfterPeriodPlanConsumeAmount);
      builder.Property(x => x.AfterPeriodPlanProduceAmount);
      builder.Property(x => x.AfterPeriodPlanSaleOrderAmount);
      builder.Property(x => x.AfterPeriodPlanPurchaseRequestAmount);
      builder.Property(x => x.AfterPeriodPlanPurchaseOrderAmount);
      builder.Property(x => x.AfterPeriodPlanCargoAmount);
      builder.Property(x => x.PlanCargoAmount);
      builder.Property(x => x.PlanConsumeAmount);
      builder.Property(x => x.PlanProduceAmount);
      builder.Property(x => x.PlanSaleOrderAmount);
      builder.Property(x => x.PlanPurchaseOrderAmount);
      builder.Property(x => x.PlanPurchaseRequestAmount);
      builder.Property(x => x.BufferRemainingAmount);
    }
  }
}
