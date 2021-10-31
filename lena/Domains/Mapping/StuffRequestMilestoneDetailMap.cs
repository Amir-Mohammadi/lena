using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffRequestMilestoneDetailMap : IEntityTypeConfiguration<StuffRequestMilestoneDetail>
  {
    public void Configure(EntityTypeBuilder<StuffRequestMilestoneDetail> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StuffRequestMilestoneDetail");
      builder.Property(x => x.Id);
      builder.Property(x => x.StuffRequestMilestoneId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.StuffRequestMilestone).WithMany(x => x.StuffRequestMilestoneDetails).HasForeignKey(x => x.StuffRequestMilestoneId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffRequestMilestoneDetails).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Unit).WithMany(x => x.StuffRequestMilestoneDetails).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.StuffRequestMilestoneDetailSummary).WithOne(x => x.StuffRequestMilestoneDetail).HasForeignKey<StuffRequestMilestoneDetailSummary>(x => x.StuffRequestMilestoneDetailId);
    }
  }
}
