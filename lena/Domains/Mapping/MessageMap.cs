using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MessageMap : IEntityTypeConfiguration<Message>
  {
    public void Configure(EntityTypeBuilder<Message> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Messages");
      builder.Property(x => x.Id);
      builder.Property(x => x.Number).IsRequired().HasMaxLength(50);
      builder.Property(x => x.SendDate).IsRequired();
      builder.Property(x => x.SenderUserId);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Content).IsRequired();
      builder.Property(x => x.IsSent);
      builder.Property(x => x.MessageAccessType);
      builder.Property(x => x.IsArchive);
      builder.Property(x => x.IsDelete);
      builder.HasRowVersion();
      builder.HasOne(x => x.SenderUser).WithMany(x => x.Messages).HasForeignKey(x => x.SenderUserId);
    }
  }
}
