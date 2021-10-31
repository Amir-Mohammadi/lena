using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseTransactionLevelMap : IEntityTypeConfiguration<WarehouseTransactionLevel>
  {
    public void Configure(EntityTypeBuilder<WarehouseTransactionLevel> builder)
    {
      builder.HasKey(x => new
      {
        x.WareHouserId,
        x.TransactionLevel
      });
      builder.ToTable("WarehouseTransactionLevels");
      builder.Property(x => x.WareHouserId);
      builder.Property(x => x.TransactionLevel).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Warehouse).WithMany(x => x.WarehouseTransactionLevels).HasForeignKey(x => x.WareHouserId);
    }
  }
}
