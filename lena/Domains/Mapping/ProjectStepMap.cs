using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectStepMap : IEntityTypeConfiguration<ProjectStep>
  {
    public void Configure(EntityTypeBuilder<ProjectStep> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ProjectStep");
      builder.Property(x => x.Id);
    }
  }
}
