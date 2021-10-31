using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPPhaseMap : IEntityTypeConfiguration<ProjectERPPhase>
  {
    public void Configure(EntityTypeBuilder<ProjectERPPhase> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPPhases");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
