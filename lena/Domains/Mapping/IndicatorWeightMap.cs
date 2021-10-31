using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class IndicatorWeightMap : IEntityTypeConfiguration<IndicatorWeight>
  {
    public void Configure(EntityTypeBuilder<IndicatorWeight> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("IndicatorWeights");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name);
      builder.Property(x => x.Code);
      builder.Property(x => x.DepartmentId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Department).WithMany(x => x.IndicatorWeights).HasForeignKey(x => x.DepartmentId);
    }
  }
}
