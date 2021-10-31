using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UserMessageRelationMap : IEntityTypeConfiguration<UserMessageRelation>
  {
    public void Configure(EntityTypeBuilder<UserMessageRelation> builder)
    {
      builder.HasKey(x => new
      {
        x.FromUserId,
        x.ToUserId
      });
      builder.ToTable("UserMessageRelations");
      builder.Property(x => x.FromUserId);
      builder.Property(x => x.ToUserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.FromUser).WithMany(x => x.ToUserMessageRelations).HasForeignKey(x => x.FromUserId);
      builder.HasOne(x => x.ToUser).WithMany(x => x.FromUserMessageRelations).HasForeignKey(x => x.ToUserId);
    }
  }
}
