using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ApplicationSettingMap : IEntityTypeConfiguration<ApplicationSetting>
  {
    public void Configure(EntityTypeBuilder<ApplicationSetting> builder)
    {
      builder.HasKey(x => x.SettingKey);
      builder.ToTable("ApplicationSettings");
      builder.Property(x => x.SettingKey);
      builder.Property(x => x.SettingKeyName).IsRequired();
      builder.Property(x => x.Value).IsRequired();
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
