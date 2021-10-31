using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RiskParameterMap : IEntityTypeConfiguration<RiskParameter>
  {
    public void Configure(EntityTypeBuilder<RiskParameter> builder)
    {
      builder.HasKey(x => new
      {
        x.OccurrenceSeverityStatus,
        x.OccurrenceProbabilityStatus
      });
      builder.ToTable("RiskParameters");
      builder.Property(x => x.OccurrenceSeverityStatus).IsRequired();
      builder.Property(x => x.OccurrenceProbabilityStatus).IsRequired();
      builder.Property(x => x.RiskLevelStatus).IsRequired();
      builder.HasRowVersion();
    }
  }
}
