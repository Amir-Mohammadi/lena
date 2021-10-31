using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionLineRepairUnitMap : IEntityTypeConfiguration<ProductionLineRepairUnit>
  {
    public void Configure(EntityTypeBuilder<ProductionLineRepairUnit> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionLineRepairUnits");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Warehouse).WithMany(x => x.ProductionLineRepairUnits).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.User).WithMany(x => x.ProductionLineRepairUnits).HasForeignKey(x => x.UserId);
    }
  }
}
