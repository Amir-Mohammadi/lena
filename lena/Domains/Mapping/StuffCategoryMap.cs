using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffCategoryMap : IEntityTypeConfiguration<StuffCategory>
  {
    public void Configure(EntityTypeBuilder<StuffCategory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffCategories");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.ParentStuffCategoryId);
      builder.HasRowVersion();
      builder.Property(x => x.DefaultWarehouseId);
      builder.HasOne(x => x.ParentStuffCategory).WithMany(x => x.SubStuffCategories).HasForeignKey(x => x.ParentStuffCategoryId);
      builder.HasOne(x => x.DefaultWarehouse).WithMany(x => x.StuffCategories).HasForeignKey(x => x.DefaultWarehouseId);
    }
  }
}