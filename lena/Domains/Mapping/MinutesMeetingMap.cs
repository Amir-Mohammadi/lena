using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MinutesMeetingMap : IEntityTypeConfiguration<MinutesMeeting>
  {
    public void Configure(EntityTypeBuilder<MinutesMeeting> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("MinutesMeetings");
      builder.Property(x => x.Id);
      builder.Property(x => x.RegistrationDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.MeetingDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Place);
      builder.Property(x => x.SecretaryUserId);
      builder.Property(x => x.RegistrantUserId);
      builder.Property(x => x.Agenda);
      builder.Property(x => x.BossUserId);
      builder.Property(x => x.IsConfidential);
      builder.HasRowVersion();
      builder.HasOne(x => x.RegistrantUser).WithMany(x => x.RegistrantMinutesMeetings).HasForeignKey(x => x.RegistrantUserId);
      builder.HasOne(x => x.BossUser).WithMany(x => x.BossMinutesMeetings).HasForeignKey(x => x.BossUserId);
      builder.HasOne(x => x.SecretaryUser).WithMany(x => x.SecretaryMinutesMeetings).HasForeignKey(x => x.SecretaryUserId);
    }
  }
}
