using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SendPermissionMap : IEntityTypeConfiguration<SendPermission>
  {
    public void Configure(EntityTypeBuilder<SendPermission> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_SendPermission");
      builder.Property(x => x.Id);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.SendPermissionStatusType);
      builder.Property(x => x.ExitReceiptRequestId);
      builder.Property(x => x.ConfirmDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.ConfirmDescription);
      builder.HasOne(x => x.Unit).WithMany(x => x.SendPermissions).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.ExitReceiptRequest).WithMany(x => x.SendPermissions).HasForeignKey(x => x.ExitReceiptRequestId);
      builder.HasOne(x => x.Confirmer).WithMany(x => x.SendPermissions).HasForeignKey(x => x.ConfirmerUserId);
    }
  }
}
