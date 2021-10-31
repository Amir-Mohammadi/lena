using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ExitReceiptDeleteRequestStuffSerialMap : IEntityTypeConfiguration<ExitReceiptDeleteRequestStuffSerial>
  {
    public void Configure(EntityTypeBuilder<ExitReceiptDeleteRequestStuffSerial> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ExitReceiptDeleteRequestStuffSerials");
      builder.Property(x => x.Id);
      builder.Property(x => x.ExitReceiptDeleteRequestId);
      builder.Property(x => x.StuffSerialId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Amount);
      builder.HasRowVersion();
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.ExitReceiptDeleteRequestStuffSerials).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffSerialId
      });
      builder.HasOne(x => x.ExitReceiptDeleteRequest).WithMany(x => x.ExitReceiptDeleteRequestStuffSerials).HasForeignKey(x => x.ExitReceiptDeleteRequestId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ExitReceiptDeleteRequestStuffSerials).HasForeignKey(x => x.UnitId);
    }
  }
}
