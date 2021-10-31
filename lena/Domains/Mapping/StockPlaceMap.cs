using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StockPlaceMap : IEntityTypeConfiguration<StockPlace>
  {
    public void Configure(EntityTypeBuilder<StockPlace> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StockPlaces");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.WarehouseId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Warehouse).WithMany(x => x.StockPlaces).HasForeignKey(x => x.WarehouseId);
    }
  }
}
