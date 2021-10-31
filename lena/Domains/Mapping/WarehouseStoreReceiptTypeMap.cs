using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseStoreReceiptTypeMap : IEntityTypeConfiguration<WarehouseStoreReceiptType>
  {
    public void Configure(EntityTypeBuilder<WarehouseStoreReceiptType> builder)
    {
      builder.HasKey(x => new
      {
        x.WarehouseId,
        x.StoreReceiptType
      });
      builder.ToTable("WarehouseSotreReceiptTypes");
      builder.Property(x => x.WarehouseId).IsRequired();
      builder.Property(x => x.StoreReceiptType).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Warehouse).WithMany(x => x.WarehouseStoreReceiptTypes).HasForeignKey(x => x.WarehouseId);
    }
  }
}
