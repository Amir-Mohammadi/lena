using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WeightDayMap : IEntityTypeConfiguration<WeightDay>
  {
    public void Configure(EntityTypeBuilder<WeightDay> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WeightDays");
      builder.Property(x => x.Id);
      builder.Property(x => x.Day).IsRequired();
      builder.Property(x => x.Amount).IsRequired();
      builder.Property(x => x.IndicatorWeightId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.IndicatorWeight).WithMany(x => x.WeightDays).HasForeignKey(x => x.IndicatorWeightId);
    }
  }
}
