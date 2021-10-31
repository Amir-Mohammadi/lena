using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlSummaryMap : IEntityTypeConfiguration<QualityControlSummary>
  {
    public void Configure(EntityTypeBuilder<QualityControlSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("QualityControlSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.AcceptedQty);
      builder.Property(x => x.FailedQty);
      builder.Property(x => x.ConditionalRequestQty);
      builder.Property(x => x.ConditionalQty);
      builder.Property(x => x.ConditionalRejectedQty);
      builder.Property(x => x.ReturnedQty);
      builder.Property(x => x.ConsumedQty);
      builder.Property(x => x.QualityControlId);
      builder.HasRowVersion();
      builder.HasOne(x => x.QualityControl).WithOne(x => x.QualityControlSummary).HasForeignKey<QualityControlSummary>(x => x.QualityControlId);
    }
  }
}