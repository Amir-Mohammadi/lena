using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StoreReceiptDeleteRequestStuffSerialMap : IEntityTypeConfiguration<StoreReceiptDeleteRequestStuffSerial>
  {
    public void Configure(EntityTypeBuilder<StoreReceiptDeleteRequestStuffSerial> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StoreReceiptDeleteRequestStuffSerials");
      builder.Property(x => x.Id);
      builder.Property(x => x.StoreReceiptDeleteRequestId);
      builder.Property(x => x.StuffSerialId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.Amount);
      builder.Property(x => x.UnitId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Unit).WithMany(x => x.StoreReceiptDeleteRequestStuffSerials).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.StoreReceiptDeleteRequestStuffSerials).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffSerialId
      });
      builder.HasOne(x => x.StoreReceiptDeleteRequest).WithMany(x => x.StoreReceiptDeleteRequestStuffSerials).HasForeignKey(x => x.StoreReceiptDeleteRequestId);
    }
  }
}
