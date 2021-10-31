using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class EquivalentStuffUsageConfirmationMap : IEntityTypeConfiguration<EquivalentStuffUsageConfirmation>
  {
    public void Configure(EntityTypeBuilder<EquivalentStuffUsageConfirmation> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_EquivalentStuffUsageConfirmation");
      builder.Property(x => x.Id);
      builder.Property(x => x.EquivalentStuffUsageId);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.EquivalentStuffUsage).WithMany(x => x.EquivalentStuffUsageConfirmations).HasForeignKey(x => x.EquivalentStuffUsageId);
    }
  }
}
