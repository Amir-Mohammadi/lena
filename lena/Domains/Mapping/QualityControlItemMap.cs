using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlItemMap : IEntityTypeConfiguration<QualityControlItem>
  {
    public void Configure(EntityTypeBuilder<QualityControlItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_QualityControlItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.QualityControlId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Status);
      builder.Property(x => x.ReturnOfSaleId);
      builder.HasOne(x => x.QualityControl).WithMany(x => x.QualityControlItems).HasForeignKey(x => x.QualityControlId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.QualityControlItems).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.QualityControlItems).HasForeignKey(x => new { x.StuffSerialCode, x.StuffId });
      builder.HasOne(x => x.QualityControlConfirmationItem).WithOne(x => x.QualityControlItem).HasForeignKey<QualityControlConfirmationItem>(x => x.QualityControlItemId);
      builder.HasOne(x => x.Unit).WithMany(x => x.QualityControlItems).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.ReturnOfSale).WithMany(x => x.QualityControlItems).HasForeignKey(x => x.ReturnOfSaleId);
    }
  }
}