using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumSprintMap : IEntityTypeConfiguration<ScrumSprint>
  {
    public void Configure(EntityTypeBuilder<ScrumSprint> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ScrumSprint");
      builder.Property(x => x.Id);
      builder.Property(x => x.ScrumProjectId);
      builder.HasOne(x => x.ScrumProject).WithMany(x => x.ScrumSprints).HasForeignKey(x => x.ScrumProjectId);
    }
  }
}
