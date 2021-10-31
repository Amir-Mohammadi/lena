using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReturnOfSaleSummaryMap : IEntityTypeConfiguration<ReturnOfSaleSummary>
  {
    public void Configure(EntityTypeBuilder<ReturnOfSaleSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ReturnOfSaleSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.QualityControlConsumedQty);
      builder.Property(x => x.ReceiptQualityControlPassedQty);
      builder.Property(x => x.ReceiptQualityControlFailedQty);
      builder.Property(x => x.ReceiptQualityControlConsumedQty);
      builder.Property(x => x.ReturnOfSaleId);
      builder.HasRowVersion();
    }
  }
}