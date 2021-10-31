using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlConfirmationItemMap : IEntityTypeConfiguration<QualityControlConfirmationItem>
  {
    public void Configure(EntityTypeBuilder<QualityControlConfirmationItem> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_QualityControlConfirmationItem");
      builder.Property(x => x.Id);
      builder.Property(x => x.RemainedQty);
      builder.Property(x => x.TestQty);
      builder.Property(x => x.ConsumeQty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.QualityControlConfirmationId);
      builder.Property(x => x.QualityControlItemId);
      builder.HasOne(x => x.Unit).WithMany(x => x.QualityControlConfirmationItems).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.QualityControlConfirmation).WithMany(x => x.QualityControlConfirmationItems).HasForeignKey(x => x.QualityControlConfirmationId);
    }
  }
}