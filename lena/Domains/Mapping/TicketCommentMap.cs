using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TicketCommentMap : IEntityTypeConfiguration<TicketComment>
  {
    public void Configure(EntityTypeBuilder<TicketComment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TicketComments");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId);
      builder.Property(x => x.TicketSoftwareId);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Content);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.TicketComments).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.TicketSoftware).WithMany(x => x.TicketComments).HasForeignKey(x => x.TicketSoftwareId);
    }
  }
}
