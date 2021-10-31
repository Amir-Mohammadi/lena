using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OrganizationPostMap : IEntityTypeConfiguration<OrganizationPost>
  {
    public void Configure(EntityTypeBuilder<OrganizationPost> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OrganizationPosts");
      builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
      builder.Property(x => x.IsActive).IsRequired();
      builder.Property(x => x.Description).IsRequired(false).HasMaxLength(512);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.IsAdmin).IsRequired();
      builder.Property(x => x.UserGroupId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Creator).WithMany(x => x.OrganizationPosts).HasForeignKey(x => x.CreatorId);
      builder.HasOne(x => x.UserGroup).WithOne(x => x.OrganizationPost).HasForeignKey<OrganizationPost>(x => x.UserGroupId);
    }
  }
}