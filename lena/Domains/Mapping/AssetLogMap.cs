using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class AssetLogMap : IEntityTypeConfiguration<AssetLog>
  {
    public void Configure(EntityTypeBuilder<AssetLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("AssetLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.AssetId);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.DepartmentId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.Description);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.Asset).WithMany(x => x.AssetLogs).HasForeignKey(x => x.AssetId);
      builder.HasOne(x => x.User).WithMany(x => x.UserAssetLogs).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Employee).WithMany(x => x.EmployeeAssetLogs).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.Department).WithMany(x => x.DepartmentAssetLogs).HasForeignKey(x => x.DepartmentId);
    }
  }
}