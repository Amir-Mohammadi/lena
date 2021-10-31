using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class NotificationMap : IEntityTypeConfiguration<Notification>
  {
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Notifications");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.IsSeen);
      builder.Property(x => x.RequestDate).HasColumnType("smalldatetime");
      builder.Property(x => x.SeenDate).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.ScrumEntityId);
      builder.HasRowVersion();
      builder.Property(x => x.NotificationGroupId);
      builder.HasOne(x => x.User).WithMany(x => x.Notifications).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.ScrumEntity).WithMany(x => x.Notifications).HasForeignKey(x => x.ScrumEntityId);
      builder.HasOne(x => x.NotificationGroup).WithMany(x => x.Notifications).HasForeignKey(x => x.NotificationGroupId);
    }
  }
}
