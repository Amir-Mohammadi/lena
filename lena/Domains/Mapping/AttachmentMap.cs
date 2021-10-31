using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class AttachmentMap : IEntityTypeConfiguration<Attachment>
  {
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Attachments");
      builder.Property(x => x.Id);
      builder.Property(x => x.MessageId);
      builder.Property(x => x.FileName).IsRequired();
      builder.Property(x => x.Size);
      builder.Property(x => x.Format).IsRequired().HasMaxLength(50);
      builder.Property(x => x.FileContent).IsRequired();
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.Message).WithMany(x => x.Attachments).HasForeignKey(x => x.MessageId);
    }
  }
}
