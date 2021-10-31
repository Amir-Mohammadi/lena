using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseRequestSummaryMap : IEntityTypeConfiguration<PurchaseRequestSummary>
  {
    public void Configure(EntityTypeBuilder<PurchaseRequestSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseRequestSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.OrderedQty);
      builder.Property(x => x.CargoedQty);
      builder.Property(x => x.ReceiptedQty);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.PurchaseRequestId);
      builder.HasRowVersion();
    }
  }
}