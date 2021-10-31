using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PostMap : IEntityTypeConfiguration<Post>
  {
    public void Configure(EntityTypeBuilder<Post> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Posts");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.PostId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ParentPost).WithMany(x => x.ChildPosts).HasForeignKey(x => x.PostId);
    }
  }
}
