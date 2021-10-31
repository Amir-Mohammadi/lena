using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UserGroupMap : IEntityTypeConfiguration<UserGroup>
  {
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UserGroups");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
