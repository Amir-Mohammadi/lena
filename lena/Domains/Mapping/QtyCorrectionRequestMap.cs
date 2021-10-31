using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QtyCorrectionRequestMap : IEntityTypeConfiguration<QtyCorrectionRequest>
  {
    public void Configure(EntityTypeBuilder<QtyCorrectionRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_QtyCorrectionRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.Type);
      builder.Property(x => x.Status);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.StockCheckingTagId);
      builder.HasOne(x => x.Unit).WithMany(x => x.QtyCorrectionRequests).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.QtyCorrectionRequests).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.QtyCorrectionRequests).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffId
      });
      builder.HasOne(x => x.Warehouse).WithMany(x => x.QtyCorrectionRequests).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.StockCheckingTag).WithMany(x => x.QtyCorrectionRequests).HasForeignKey(x => x.StockCheckingTagId);
    }
  }
}
