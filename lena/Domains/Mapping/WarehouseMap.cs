using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseMap : IEntityTypeConfiguration<Warehouse>
  {
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Warehouses");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.IsDeleted);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.FIFO);
      builder.Property(x => x.DisplayOrder);
      builder.Property(x => x.WarehouseType);
      builder.HasOne(x => x.Department).WithMany(x => x.Warehouses).HasForeignKey(x => x.DepartmentId);
    }
  }
}
