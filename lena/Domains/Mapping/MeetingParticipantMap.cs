using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MeetingParticipantMap : IEntityTypeConfiguration<MeetingParticipant>
  {
    public void Configure(EntityTypeBuilder<MeetingParticipant> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("MeetingParticipants");
      builder.Property(x => x.Id);
      builder.Property(x => x.MinutesMeetingId);
      builder.Property(x => x.ParticipantEmployeeId);
      builder.Property(x => x.GuestParticipantName);
      builder.HasRowVersion();
      builder.HasOne(x => x.MinutesMeeting).WithMany(x => x.MeetingParticipants).HasForeignKey(x => x.MinutesMeetingId);
      builder.HasOne(x => x.ParticipantEmployee).WithMany(x => x.MeetingParticipants).HasForeignKey(x => x.ParticipantEmployeeId);
    }
  }
}
