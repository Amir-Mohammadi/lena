using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialMap : IEntityTypeConfiguration<BillOfMaterial>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterial> builder)
    {
      builder.HasKey(x => new
      {
        x.Version,
        x.StuffId
      });
      builder.ToTable("BillOfMaterials");
      builder.Property(x => x.StuffId);
      builder.Property(x => x.Version);
      builder.Property(x => x.ProductionStepId);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.BillOfMaterialVersionType);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.IsPublished);
      builder.Property(x => x.CreateDate).HasColumnType("smalldatetime");
      builder.Property(x => x.Value);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.QtyPerBox);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.UserId);
      builder.HasOne(x => x.Unit).WithMany(x => x.BillOfMaterials).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.BillOfMaterials).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.ProductionStep).WithMany(x => x.BillOfMaterials).HasForeignKey(x => x.ProductionStepId);
      builder.HasOne(x => x.User).WithMany(x => x.BillOfMaterials).HasForeignKey(x => x.UserId);
    }
  }
}
