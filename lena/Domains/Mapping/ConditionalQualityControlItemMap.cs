using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ConditionalQualityControlItemMap : IEntityTypeConfiguration<ConditionalQualityControlItem>
  {
    public void Configure(EntityTypeBuilder<ConditionalQualityControlItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ConditionalQualityControlItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.ConditionalQualityControlId);
      builder.Property(x => x.QualityControlConfirmationItemId);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.HasOne(x => x.ConditionalQualityControl).WithMany(x => x.ConditionalQualityControlItems).HasForeignKey(x => x.ConditionalQualityControlId);
      builder.HasOne(x => x.QualityControlConfirmationItem).WithMany(x => x.ConditionalQualityControlItems).HasForeignKey(x => x.QualityControlConfirmationItemId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ConditionalQualityControlItems).HasForeignKey(x => x.UnitId);
    }
  }
}
