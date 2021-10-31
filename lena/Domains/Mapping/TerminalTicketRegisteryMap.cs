using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TerminalTicketRegisteryMap : IEntityTypeConfiguration<TerminalTicketRegistery>
  {
    public void Configure(EntityTypeBuilder<TerminalTicketRegistery> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TerminalTicketRegisteries");
      builder.Property(x => x.Id);
      builder.Property(x => x.Date).HasColumnType("smalldatetime");
      builder.Property(x => x.SessionId).IsRequired();
      builder.Property(x => x.Value).IsRequired();
      builder.HasRowVersion();
    }
  }
}
