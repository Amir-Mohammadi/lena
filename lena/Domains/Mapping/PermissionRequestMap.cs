using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PermissionRequestMap : IEntityTypeConfiguration<PermissionRequest>
  {
    public void Configure(EntityTypeBuilder<PermissionRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PermissionRequests");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
      builder.Property(x => x.RegistrarUserId);
      builder.Property(x => x.RegisterDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IntendedUserId);
      builder.HasOne(x => x.RegistrarUser).WithMany(x => x.PermissionRequestRegisterars).HasForeignKey(x => x.RegistrarUserId);
      builder.HasOne(x => x.IntendedUser).WithMany(x => x.PermissionRequestIntenders).HasForeignKey(x => x.IntendedUserId);
    }
  }
}