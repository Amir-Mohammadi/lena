using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseStuffTypeMap : IEntityTypeConfiguration<WarehouseStuffType>
  {
    public void Configure(EntityTypeBuilder<WarehouseStuffType> builder)
    {
      builder.HasKey(x => new
      {
        x.WarehouseId,
        x.StuffType
      });
      builder.ToTable("WarehouseStuffTypes");
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.StuffType).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Warehouse).WithMany(x => x.WarehouseStuffTypes).HasForeignKey(x => x.WarehouseId);
    }
  }
}
