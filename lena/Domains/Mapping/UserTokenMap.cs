using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UserTokenMap : IEntityTypeConfiguration<UserToken>
  {
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserTokens");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.Token).IsRequired();
      builder.Property(x => x.RefreshToken).IsRequired();
      builder.Property(x => x.ExpiresIn).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.UserTokens).HasForeignKey(x => x.UserId);
    }
  }
}
