using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumProjectMap : IEntityTypeConfiguration<ScrumProject>
  {
    public void Configure(EntityTypeBuilder<ScrumProject> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ScrumProject");
      builder.Property(x => x.Id);
      builder.Property(x => x.ScrumProjectGroupId);
      builder.HasOne(x => x.ScrumProjectGroup).WithMany(x => x.ScrumProjects).HasForeignKey(x => x.ScrumProjectGroupId);
    }
  }
}
