using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlConfirmationMap : IEntityTypeConfiguration<QualityControlConfirmation>
  {
    public void Configure(EntityTypeBuilder<QualityControlConfirmation> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_QualityControlConfirmation");
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.IsEmergency);
      builder.Property(x => x.QualityControlId);
    }
  }
}