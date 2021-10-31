using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UserMap : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Users");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserName).IsRequired().HasMaxLength(50);
      builder.Property(x => x.Password).IsRequired().HasMaxLength(512);
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.IsLocked);
      builder.Property(x => x.LockOutDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.LoginFailedCount);
      builder.HasRowVersion();
      builder.Property(x => x.PasswordExpirationDate).HasColumnType("smalldatetime");
      builder.Property(x => x.HasAccessFromInternet);
    }
  }
}
