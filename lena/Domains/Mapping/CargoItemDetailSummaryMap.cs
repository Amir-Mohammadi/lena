using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CargoItemDetailSummaryMap : IEntityTypeConfiguration<CargoItemDetailSummary>
  {
    public void Configure(EntityTypeBuilder<CargoItemDetailSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CargoItemDetailSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.ReceiptedQty);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.LadingItemDetailQty);
      builder.Property(x => x.CargoItemDetailId);
      builder.HasRowVersion();
    }
  }
}