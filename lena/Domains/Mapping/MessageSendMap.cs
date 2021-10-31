using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MessageSendMap : IEntityTypeConfiguration<MessageSend>
  {
    public void Configure(EntityTypeBuilder<MessageSend> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("MessageSends");
      builder.Property(x => x.Id);
      builder.Property(x => x.MessageId);
      builder.Property(x => x.ReciverUserId);
      builder.Property(x => x.MessageSendType);
      builder.Property(x => x.IsRead);
      builder.Property(x => x.ReadDate).HasColumnType("smalldatetime");
      builder.Property(x => x.IsArchive);
      builder.Property(x => x.IsDelete);
      builder.HasRowVersion();
      builder.HasOne(x => x.ReciverUser).WithMany(x => x.MessageSends).HasForeignKey(x => x.ReciverUserId);
    }
  }
}
