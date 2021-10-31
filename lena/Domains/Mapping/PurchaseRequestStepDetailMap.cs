using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseRequestStepDetailMap : IEntityTypeConfiguration<PurchaseRequestStepDetail>
  {
    public void Configure(EntityTypeBuilder<PurchaseRequestStepDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseRequestStepDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.UserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description).IsRequired(false);
      builder.Property(x => x.PurchaseRequestStepId);
      builder.HasRowVersion();
      builder.Property(x => x.PurchaseRequestId);
      builder.Property(x => x.DocumentId);
      builder.HasOne(x => x.User).WithMany(x => x.PurchaseRequestStepDetails).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.PurchaseRequestStep).WithMany(x => x.PurchaseRequestStepDetails).HasForeignKey(x => x.PurchaseRequestStepId);
      builder.HasOne(x => x.PurchaseRequest).WithMany(x => x.PurchaseRequestStepDetails).HasForeignKey(x => x.PurchaseRequestId);
    }
  }
}
