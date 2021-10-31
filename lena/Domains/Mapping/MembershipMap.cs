using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class MembershipMap : IEntityTypeConfiguration<Membership>
  {
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Memberships");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId);
      builder.Property(x => x.UserGroupId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.Memberships).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.UserGroup).WithMany(x => x.Memberships).HasForeignKey(x => x.UserGroupId);
    }
  }
}
