using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ExitReceiptRequestSummaryMap : IEntityTypeConfiguration<ExitReceiptRequestSummary>
  {
    public void Configure(EntityTypeBuilder<ExitReceiptRequestSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ExitReceiptRequestSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.PermissionQty);
      builder.Property(x => x.PreparingSendingQty);
      builder.Property(x => x.SendedQty);
      builder.Property(x => x.ExitReceiptRequestId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ExitReceiptRequest).WithOne(x => x.ExitReceiptRequestSummary).HasForeignKey<ExitReceiptRequestSummary>(x => x.ExitReceiptRequestId);
    }
  }
}