using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class LadingCustomhouseStatusMap : IEntityTypeConfiguration<LadingCustomhouseStatus>
  {
    public void Configure(EntityTypeBuilder<LadingCustomhouseStatus> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("LadingCustomhouseStatuses");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
    }
  }
}
