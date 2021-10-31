using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CargoItemSummaryMap : IEntityTypeConfiguration<CargoItemSummary>
  {
    public void Configure(EntityTypeBuilder<CargoItemSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("CargoItemSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.ReceiptedQty);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.CargoItemId);
      builder.Property(x => x.LadingItemQty);
      builder.HasRowVersion();
    }
  }
}