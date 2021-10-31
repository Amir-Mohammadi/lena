using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SecurityActionGroupMap : IEntityTypeConfiguration<SecurityActionGroup>
  {
    public void Configure(EntityTypeBuilder<SecurityActionGroup> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SecurityActionGroups");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.DisplayName).IsRequired();
      builder.HasRowVersion();
    }
  }
}
