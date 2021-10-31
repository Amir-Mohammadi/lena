using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlTestUnitMap : IEntityTypeConfiguration<QualityControlTestUnit>
  {
    public void Configure(EntityTypeBuilder<QualityControlTestUnit> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("QualityControlTestUnits");
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description).IsRequired(false);
      builder.Property(x => x.IsActive);
      builder.HasRowVersion();
    }
  }
}