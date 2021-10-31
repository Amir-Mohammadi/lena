using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderStepMap : IEntityTypeConfiguration<PurchaseOrderStep>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderStep> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseOrderSteps");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.AllowUploadDocument);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.PurchaseOrderSteps).HasForeignKey(x => x.UserId);
    }
  }
}
