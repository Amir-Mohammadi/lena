using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UserSettingMap : IEntityTypeConfiguration<UserSetting>
  {
    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserSettings");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId);
      builder.Property(x => x.Key).IsRequired();
      builder.Property(x => x.Value).IsRequired();
      builder.Property(x => x.ValueType);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.UserSettings).HasForeignKey(x => x.UserId);
    }
  }
}
