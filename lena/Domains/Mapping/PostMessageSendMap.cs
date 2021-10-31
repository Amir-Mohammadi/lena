using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PostMessageSendMap : IEntityTypeConfiguration<PostMessageSend>
  {
    public void Configure(EntityTypeBuilder<PostMessageSend> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("MessageSends_PostMessageSend");
      builder.Property(x => x.Id);
      builder.Property(x => x.PostId);
      builder.HasOne(x => x.Post).WithMany(x => x.PostMessageSends).HasForeignKey(x => x.PostId);
    }
  }
}
