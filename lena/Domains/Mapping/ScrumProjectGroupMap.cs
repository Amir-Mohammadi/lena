using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumProjectGroupMap : IEntityTypeConfiguration<ScrumProjectGroup>
  {
    public void Configure(EntityTypeBuilder<ScrumProjectGroup> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ScrumProjectGroup");
      builder.Property(x => x.Id);
    }
  }
}
