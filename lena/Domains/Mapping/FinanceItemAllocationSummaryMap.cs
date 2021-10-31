using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinanceItemAllocationSummaryMap : IEntityTypeConfiguration<FinanceItemAllocationSummary>
  {
    public void Configure(EntityTypeBuilder<FinanceItemAllocationSummary> builder)
    {
      builder.HasKey(x => new
      {
        x.FinanceId,
        x.CooperatorId
      });
      builder.ToTable("FinanceItemAllocationSummaries");
      builder.Property(x => x.FinanceId).IsRequired();
      builder.Property(x => x.CooperatorId).IsRequired();
      builder.Property(x => x.TotalRequestedAmout).IsRequired();
      builder.Property(x => x.TotalAllocatedAmount).IsRequired();
      builder.Property(x => x.TotalTransferredAmount).IsRequired();
      builder.Property(x => x.CooperatorId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Cooperator).WithMany(x => x.FinanceItemAllocationSummaries).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.Finance).WithMany(x => x.FinanceItemAllocationSummaries).HasForeignKey(x => x.FinanceId);//TODO fix it .WillCascadeOnDelete(true);
    }
  }
}
