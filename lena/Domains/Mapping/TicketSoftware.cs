using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TicketSoftwareMap : IEntityTypeConfiguration<TicketSoftware>
  {
    public void Configure(EntityTypeBuilder<TicketSoftware> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TicketSoftwares");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId);
      builder.Property(x => x.Subject);
      builder.Property(x => x.Content);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UpdateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Status);
      builder.Property(x => x.IssueLink);
      builder.Property(x => x.Priority);
      builder.Property(x => x.LastedEditorUserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.CreatorOfTicketSoftwares).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.LastedEditorUser).WithMany(x => x.ModifierOfTicketSoftwares).HasForeignKey(x => x.LastedEditorUserId);
    }
  }
}