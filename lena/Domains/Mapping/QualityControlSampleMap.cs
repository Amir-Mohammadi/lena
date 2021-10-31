using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class QualityControlSampleMap : IEntityTypeConfiguration<QualityControlSample>
  {
    public void Configure(EntityTypeBuilder<QualityControlSample> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("QualityControlSamples");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.UserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.StatusChangerUserId).IsRequired(false);
      builder.Property(x => x.Qty);
      builder.Property(x => x.TestQty).IsRequired(false);
      builder.Property(x => x.ConsumeQty).IsRequired(false);
      builder.Property(x => x.Status);
      builder.HasRowVersion();
      builder.Property(x => x.QualityControlItemId);
      builder.HasOne(x => x.QualityControlItem).WithMany(x => x.QualityControlSamples).HasForeignKey(x => x.QualityControlItemId);
      builder.HasOne(x => x.User).WithMany(x => x.QualityControlSamples).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.StatusChangerUser).WithMany(x => x.StatusChangerQualityControlSample).HasForeignKey(x => x.StatusChangerUserId);
    }
  }
}
