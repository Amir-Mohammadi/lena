using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumEntityDependencyMap : IEntityTypeConfiguration<ScrumEntityDependency>
  {
    public void Configure(EntityTypeBuilder<ScrumEntityDependency> builder)
    {
      builder.HasKey(x => new
      {
        x.RequisiteScrumEntityId,
        x.NextScrumEntityId
      });
      builder.ToTable("ScrumEntityDependencies");
      builder.Property(x => x.RequisiteScrumEntityId);
      builder.Property(x => x.NextScrumEntityId);
      builder.HasRowVersion();
      builder.HasOne(x => x.NextScrumEntity).WithMany(x => x.RequisiteScrumEntityDependencies).HasForeignKey(x => x.NextScrumEntityId);
      builder.HasOne(x => x.RequisiteScrumEntity).WithMany(x => x.NextScrumEntityDependencies).HasForeignKey(x => x.RequisiteScrumEntityId);
    }
  }
}
