using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StoreReceiptSummaryMap : IEntityTypeConfiguration<StoreReceiptSummary>
  {
    public void Configure(EntityTypeBuilder<StoreReceiptSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StoreReceiptSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.QualityControlConsumedQty);
      builder.Property(x => x.ReceiptQualityControlPassedQty);
      builder.Property(x => x.ReceiptQualityControlFailedQty);
      builder.Property(x => x.ReceiptQualityControlConsumedQty);
      builder.Property(x => x.StoreReceiptId);
      builder.Property(x => x.PayedAmount);
      builder.HasRowVersion();
    }
  }
}