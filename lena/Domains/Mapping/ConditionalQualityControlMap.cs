using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ConditionalQualityControlMap : IEntityTypeConfiguration<ConditionalQualityControl>
  {
    public void Configure(EntityTypeBuilder<ConditionalQualityControl> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ConditionalQualityControl");
      builder.Property(x => x.Id);
      builder.Property(x => x.QualityControlAccepterId);
      builder.Property(x => x.QualityControlConfirmationId);
      builder.Property(x => x.WarrantyExpirationExceptionTypeId);
      builder.Property(x => x.ResponseConditionalConfirmationDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ResponseConditionalConfirmationUserId);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.QualityControlAccepter).WithMany(x => x.ConditionalQualityControls).HasForeignKey(x => x.QualityControlAccepterId);
      builder.HasOne(x => x.QualityControlConfirmation).WithMany(x => x.ConditionalQualityControls).HasForeignKey(x => x.QualityControlConfirmationId);
      builder.HasOne(x => x.WarrantyExpirationExceptionType).WithMany(x => x.ConditionalQualityControls).HasForeignKey(x => x.WarrantyExpirationExceptionTypeId);
      builder.HasOne(x => x.ResponseConditionalConfirmationlUser).WithMany(x => x.ConditionalQualityControls).HasForeignKey(x => x.ResponseConditionalConfirmationUserId);
    }
  }
}
