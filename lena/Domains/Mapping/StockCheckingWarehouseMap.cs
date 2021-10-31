using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StockCheckingWarehouseMap : IEntityTypeConfiguration<StockCheckingWarehouse>
  {
    public void Configure(EntityTypeBuilder<StockCheckingWarehouse> builder)
    {
      builder.HasKey(x => new
      {
        x.StockCheckingId,
        x.WarehouseId
      });
      builder.ToTable("StockCheckingWarehouses");
      builder.Property(x => x.StockCheckingId);
      builder.Property(x => x.WarehouseId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StockChecking).WithMany(x => x.StockCheckingWarehouses).HasForeignKey(x => x.StockCheckingId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.StockCheckingWarehouses).HasForeignKey(x => x.WarehouseId);
    }
  }
}
