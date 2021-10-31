using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TicketFileMap : IEntityTypeConfiguration<TicketFile>
  {
    public void Configure(EntityTypeBuilder<TicketFile> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TicketFiles");
      builder.Property(x => x.Id);
      builder.Property(x => x.TicketSoftWareId);
      builder.Property(x => x.DocumentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.TicketSoftware).WithMany(x => x.TicketFiles).HasForeignKey(x => x.TicketSoftWareId);
    }
  }
}

