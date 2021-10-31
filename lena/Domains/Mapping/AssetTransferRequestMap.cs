using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class AssetTransferRequestMap : IEntityTypeConfiguration<AssetTransferRequest>
  {
    public void Configure(EntityTypeBuilder<AssetTransferRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("AssetTransferRequests");
      builder.Property(x => x.Id);
      builder.Property(x => x.AssetId);
      builder.Property(x => x.NewEmployeeId);
      builder.Property(x => x.NewDepartmentId);
      builder.Property(x => x.RequestingUserId);
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.Description);
      builder.Property(x => x.ConfirmDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.RequestDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Status);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.Asset).WithMany(x => x.AssetTransferRequests).HasForeignKey(x => x.AssetId);
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.ConfirmerUserAssetTransfers).HasForeignKey(x => x.ConfirmerUserId);
      builder.HasOne(x => x.RequestingUser).WithMany(x => x.RequestingUserAssetTransfers).HasForeignKey(x => x.RequestingUserId);
      builder.HasOne(x => x.NewEmployee).WithMany(x => x.NewEmployeeAssetTransferRequests).HasForeignKey(x => x.NewEmployeeId);
      builder.HasOne(x => x.NewDepartment).WithMany(x => x.NewDepartmentAssetTransferRequests).HasForeignKey(x => x.NewDepartmentId);
    }
  }
}