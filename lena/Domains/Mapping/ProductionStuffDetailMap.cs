using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionStuffDetailMap : IEntityTypeConfiguration<ProductionStuffDetail>
  {
    public void Configure(EntityTypeBuilder<ProductionStuffDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionStuffDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionId);
      builder.Property(x => x.ProductionOperationId);
      builder.Property(x => x.BillOfMaterialDetailType);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.HasRowVersion();
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.Type);
      builder.Property(x => x.DetachedQty);
      builder.HasOne(x => x.Production).WithMany(x => x.ProductionStuffDetails).HasForeignKey(x => x.ProductionId);
      builder.HasOne(x => x.ProductionOperation).WithMany(x => x.ProductionStuffDetails).HasForeignKey(x => x.ProductionOperationId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.ProductionStuffDetails).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.ProductionStuffDetails).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffId
      });
      builder.HasOne(x => x.Unit).WithMany(x => x.ProductionStuffDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.ProductionStuffDetails).HasForeignKey(x => x.WarehouseId);
    }
  }
}
