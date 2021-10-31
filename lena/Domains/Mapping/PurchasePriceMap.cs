using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchasePriceMap : IEntityTypeConfiguration<PurchasePrice>
  {
    public void Configure(EntityTypeBuilder<PurchasePrice> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PurchasePrice");
      builder.Property(x => x.Id);
      builder.Property(x => x.StoreReceiptId);
      builder.Property(x => x.CurrencyRate);
      builder.Property(x => x.RialPrice);
      builder.Property(x => x.DutyCost);
      builder.Property(x => x.TransferCost);
      builder.Property(x => x.OtherCost);
      builder.Property(x => x.Discount);
      builder.HasOne(x => x.StoreReceipt).WithMany(x => x.PurchasePrices).HasForeignKey(x => x.StoreReceiptId);
    }
  }
}
