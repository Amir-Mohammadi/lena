using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseExitReceiptTypeMap : IEntityTypeConfiguration<WarehouseExitReceiptType>
  {
    public void Configure(EntityTypeBuilder<WarehouseExitReceiptType> builder)
    {
      builder.HasKey(x => new
      {
        x.WarehouseId,
        x.ExitReceiptRequestTypeId
      });
      builder.ToTable("WarehouseExitReceiptTypes");
      builder.Property(x => x.WarehouseId).IsRequired();
      builder.Property(x => x.ExitReceiptRequestTypeId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Warehouse).WithMany(x => x.WarehouseExitReceiptTypes).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.ExitReceiptRequestType).WithMany(x => x.WarehouseExitReceiptTypes).HasForeignKey(x => x.ExitReceiptRequestTypeId);
    }
  }
}
