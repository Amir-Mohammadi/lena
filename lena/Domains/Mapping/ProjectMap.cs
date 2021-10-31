using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectMap : IEntityTypeConfiguration<Project>
  {
    public void Configure(EntityTypeBuilder<Project> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_Project");
      builder.Property(x => x.Id);
    }
  }
}
