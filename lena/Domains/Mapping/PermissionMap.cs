using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PermissionMap : IEntityTypeConfiguration<Permission>
  {
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Permissions");
      builder.Property(x => x.Id);
      builder.Property(x => x.SecurityActionId);
      builder.Property(x => x.UserGroupId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.AccessType);
      builder.HasRowVersion();
      builder.HasOne(x => x.SecurityAction).WithMany(x => x.Permissions).HasForeignKey(x => x.SecurityActionId);
      builder.HasOne(x => x.UserGroup).WithMany(x => x.Permissions).HasForeignKey(x => x.UserGroupId);
      builder.HasOne(x => x.User).WithMany(x => x.Permissions).HasForeignKey(x => x.UserId);
    }
  }
}
