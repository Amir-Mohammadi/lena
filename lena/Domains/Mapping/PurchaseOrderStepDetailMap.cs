using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderStepDetailMap : IEntityTypeConfiguration<PurchaseOrderStepDetail>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderStepDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseOrderStepDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.PurchaseOrderStepId);
      builder.Property(x => x.Description).IsRequired(false);
      builder.HasRowVersion();
      builder.Property(x => x.PurchaseOrderId);
      builder.Property(x => x.DocumentId);
      builder.HasOne(x => x.User).WithMany(x => x.PurchaseOrderStepDetails).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.PurchaseOrderStep).WithMany(x => x.PurchaseOrderStepDetails).HasForeignKey(x => x.PurchaseOrderStepId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.PurchaseOrderStepDetails).HasForeignKey(x => x.PurchaseOrderId);
    }
  }
}
