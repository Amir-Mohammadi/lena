using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReturnOfSaleMap : IEntityTypeConfiguration<ReturnOfSale>
  {
    public void Configure(EntityTypeBuilder<ReturnOfSale> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ReturnOfSale");
      builder.Property(x => x.Id);
      builder.Property(x => x.Type);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Status);
      builder.Property(x => x.ReturnStoreReceiptId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.SendProductId);
      builder.Property(x => x.MainStuffId);
      builder.Property(x => x.ExitReceiptCode);
      builder.HasOne(x => x.ReturnStoreReceipt).WithMany(x => x.ReturnOfSales).HasForeignKey(x => x.ReturnStoreReceiptId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.ReturnOfSales).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffId
      });
      builder.HasOne(x => x.Stuff).WithMany(x => x.ReturnOfSales).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.SendProduct).WithMany(x => x.ReturnOfSales).HasForeignKey(x => x.SendProductId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ReturnOfSales).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.ReturnOfSaleSummary).WithOne(x => x.ReturnOfSale).HasForeignKey<ReturnOfSaleSummary>(x => x.ReturnOfSaleId);
    }
  }
}
