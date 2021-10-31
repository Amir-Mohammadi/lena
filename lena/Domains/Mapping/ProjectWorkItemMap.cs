using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectWorkItemMap : IEntityTypeConfiguration<ProjectWorkItem>
  {
    public void Configure(EntityTypeBuilder<ProjectWorkItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ProjectWorkItem");
      builder.Property(x => x.Id);
    }
  }
}
