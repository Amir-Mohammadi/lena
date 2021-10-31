using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionLineMap : IEntityTypeConfiguration<ProductionLine>
  {
    public void Configure(EntityTypeBuilder<ProductionLine> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionLines");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.DetailedCode).IsRequired();
      builder.Property(x => x.ConfirmationDetailedCode).IsRequired();
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.ProductWarehouseId);
      builder.Property(x => x.ConsumeWarehouseId);
      builder.Property(x => x.SortIndex);
      builder.Property(x => x.ProductivityImpactFactor);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.AdminUserGroupId);
      builder.Property(x => x.ProductionLineRepairUnitId);
      builder.HasOne(x => x.ProductWarehouse).WithMany(x => x.ProducerProductionLines).HasForeignKey(x => x.ProductWarehouseId);
      builder.HasOne(x => x.ConsumeWarehouse).WithMany(x => x.ConsumerProductionLines).HasForeignKey(x => x.ConsumeWarehouseId);
      builder.HasOne(x => x.Department).WithMany(x => x.ProductionLines).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.AdminUserGroup).WithMany(x => x.ProductionLines).HasForeignKey(x => x.AdminUserGroupId);
      builder.HasOne(x => x.ProductionLineRepairUnit).WithMany(x => x.ProductionLines).HasForeignKey(x => x.ProductionLineRepairUnitId);
    }
  }
}
