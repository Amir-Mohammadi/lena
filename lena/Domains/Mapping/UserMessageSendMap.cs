using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UserMessageSendMap : IEntityTypeConfiguration<UserMessageSend>
  {
    public void Configure(EntityTypeBuilder<UserMessageSend> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("MessageSends_UserMessageSend");
      builder.Property(x => x.Id);
    }
  }
}
