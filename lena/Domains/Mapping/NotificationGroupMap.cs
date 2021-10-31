using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class NotificationGroupMap : IEntityTypeConfiguration<NotificationGroup>
  {
    public void Configure(EntityTypeBuilder<NotificationGroup> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("NotificationGroups");
      builder.Property(x => x.Id);
      builder.HasRowVersion();
    }
  }
}
