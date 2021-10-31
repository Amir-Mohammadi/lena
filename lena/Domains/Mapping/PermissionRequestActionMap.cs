using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PermissionRequestActionMap : IEntityTypeConfiguration<PermissionRequestAction>
  {
    public void Configure(EntityTypeBuilder<PermissionRequestAction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PermissionRequestActions");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.AccessType);
      builder.Property(x => x.PermissionRequestId);
      builder.Property(x => x.ConfirmationUserId);
      builder.Property(x => x.SecurityActionId);
      builder.Property(x => x.Description);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.PermissionRequest).WithMany(x => x.PermissionRequestActions).HasForeignKey(x => x.PermissionRequestId);
      builder.HasOne(x => x.ConfirmationUser).WithMany(x => x.PermissionRequestConfirmators).HasForeignKey(x => x.ConfirmationUserId);
      builder.HasOne(x => x.SecurityAction).WithMany(x => x.PermissionRequestActions).HasForeignKey(x => x.SecurityActionId);
    }
  }
}