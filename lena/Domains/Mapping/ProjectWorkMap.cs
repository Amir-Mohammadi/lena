using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectWorkMap : IEntityTypeConfiguration<ProjectWork>
  {
    public void Configure(EntityTypeBuilder<ProjectWork> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ProjectWork");
      builder.Property(x => x.Id);
    }
  }
}
