using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffStockPlaceMap : IEntityTypeConfiguration<StuffStockPlace>
  {
    public void Configure(EntityTypeBuilder<StuffStockPlace> builder)
    {
      builder.HasKey(x => new { x.StuffId, x.StockPlaceId });
      builder.ToTable("StuffStockPlaces");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StockPlaceId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffStockPlaces).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.StockPlace).WithMany(x => x.StuffStockPlaces).HasForeignKey(x => x.StockPlaceId);
    }
  }
}