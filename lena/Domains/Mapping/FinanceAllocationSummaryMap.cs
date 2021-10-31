using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinanceAllocationSummaryMap : IEntityTypeConfiguration<FinanceAllocationSummary>
  {
    public void Configure(EntityTypeBuilder<FinanceAllocationSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinanceAllocationSummaries");
      builder.Property(x => x.RequestedAmount).IsRequired();
      builder.Property(x => x.AllocatedAmount).IsRequired();
      builder.Property(x => x.SeparatedTransferAmount).IsRequired();
      builder.Property(x => x.TransferredAmount).IsRequired();
      builder.Property(x => x.FinanceId).IsRequired();
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.Finance).WithOne(x => x.FinanceAllocationSummary).HasForeignKey<FinanceAllocationSummary>(x => x.FinanceId);
    }
  }
}