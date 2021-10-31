using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MeetingApprovalMap : IEntityTypeConfiguration<MeetingApproval>
  {
    public void Configure(EntityTypeBuilder<MeetingApproval> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("MeetingApprovals");
      builder.Property(x => x.Id);
      builder.Property(x => x.MinutesMeetingId);
      builder.Property(x => x.ActionDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ActualDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.OperatorUserId);
      builder.Property(x => x.GuestOperatorName);
      builder.Property(x => x.Status);
      builder.Property(x => x.Title);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.OperatorDepartmentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.MinutesMeeting).WithMany(x => x.MeetingAprovals).HasForeignKey(x => x.MinutesMeetingId);
      builder.HasOne(x => x.OperatorUser).WithMany(x => x.OperatorMeetingAprovals).HasForeignKey(x => x.OperatorUserId);
      builder.HasOne(x => x.Department).WithMany(x => x.MeetingApprovals).HasForeignKey(x => x.OperatorDepartmentId);
    }
  }
}
