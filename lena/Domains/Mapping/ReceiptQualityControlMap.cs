using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReceiptQualityControlMap : IEntityTypeConfiguration<ReceiptQualityControl>
  {
    public void Configure(EntityTypeBuilder<ReceiptQualityControl> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ReceiptQualityControl");
      builder.Property(x => x.Id);
      builder.Property(x => x.StoreReceiptId);
      builder.HasOne(x => x.StoreReceipt).WithMany(x => x.ReceiptQualityControls).HasForeignKey(x => x.StoreReceiptId);
    }
  }
}
