using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class AssetMap : IEntityTypeConfiguration<Asset>
  {
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Assets");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.Status);
      builder.Property(x => x.Description);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffAssets).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.User).WithMany(x => x.UserAssets).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Employee).WithMany(x => x.EmployeeAssets).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.Department).WithMany(x => x.DepartmentAssets).HasForeignKey(x => x.DepartmentId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.WarehouseAssets).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.StuffSerial).WithOne(x => x.Asset).HasForeignKey<Asset>(x => new { x.StuffSerialCode, x.StuffId });
    }
  }
}