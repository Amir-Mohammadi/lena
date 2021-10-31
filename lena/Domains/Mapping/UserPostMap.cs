using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UserPostMap : IEntityTypeConfiguration<UserPost>
  {
    public void Configure(EntityTypeBuilder<UserPost> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserPosts");
      builder.Property(x => x.Id);
      builder.Property(x => x.PostId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.Date).HasColumnType("smalldatetime");
      builder.Property(x => x.DeleteDate).HasColumnType("smalldatetime");
      builder.Property(x => x.IsDelete);
      builder.HasRowVersion();
      builder.HasOne(x => x.Post).WithMany(x => x.UserPosts).HasForeignKey(x => x.PostId);
      builder.HasOne(x => x.User).WithMany(x => x.UserPosts).HasForeignKey(x => x.UserId);
    }
  }
}
