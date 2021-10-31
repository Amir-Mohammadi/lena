using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectHeaderMap : IEntityTypeConfiguration<ProjectHeader>
  {
    public void Configure(EntityTypeBuilder<ProjectHeader> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ProjectHeader");
      builder.Property(x => x.Id);
      builder.Property(x => x.OwnerCustomerId);
      builder.HasOne(x => x.OwnerCustomer).WithMany(x => x.ProjectHeaders).HasForeignKey(x => x.OwnerCustomerId);
    }
  }
}
