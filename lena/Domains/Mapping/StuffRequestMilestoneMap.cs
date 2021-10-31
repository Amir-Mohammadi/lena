using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffRequestMilestoneMap : IEntityTypeConfiguration<StuffRequestMilestone>
  {
    public void Configure(EntityTypeBuilder<StuffRequestMilestone> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StuffRequestMilestone");
      builder.Property(x => x.Id);
      builder.Property(x => x.DueDate).HasColumnType("smalldatetime");
      builder.Property(x => x.IsClosed);
    }
  }
}
