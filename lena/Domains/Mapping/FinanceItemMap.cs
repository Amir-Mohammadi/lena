using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinanceItemMap : IEntityTypeConfiguration<FinanceItem>
  {
    public void Configure(EntityTypeBuilder<FinanceItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinanceItems");
      builder.Property(x => x.Id);
      builder.Property(x => x.ChequeNumber).IsRequired(false).HasMaxLength(200);
      builder.Property(x => x.FinancialDescription).IsRequired(false).HasMaxLength(300);
      builder.Property(x => x.FinanceId).IsRequired(false);
      builder.Property(x => x.PurchaseOrderId).IsRequired(false);
      builder.Property(x => x.PurchaseOrderId).IsRequired(false);
      builder.Property(x => x.CooperatorId).IsRequired();
      builder.Property(x => x.PaymentMethod).IsRequired();
      builder.Property(x => x.PaymentKind).IsRequired(false);
      builder.Property(x => x.ExpenseFinancialDocumentId).IsRequired(false);
      builder.Property(x => x.FinanceType).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.RequestedAmount).IsRequired();
      builder.Property(x => x.RequestedDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.AcceptedDueDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.Property(x => x.AllocatedAmount).IsRequired(false);
      builder.Property(x => x.Description).IsRequired(false);
      builder.Property(x => x.AcceptedPaymentMethod).IsRequired(false);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.Property(x => x.ReceivedCreatedDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.Property(x => x.ReceivedDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.Property(x => x.ReceivedUserId).IsRequired(false);
      builder.Property(x => x.LatestConfirmationId);
      builder.HasOne(x => x.User).WithMany(x => x.FinanceItems).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.FinanceItems).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.LatestFinanceItemConfirmation).WithOne(x => x.LatestFinanceItem).HasForeignKey<FinanceItem>(x => x.LatestConfirmationId);
      builder.HasOne(x => x.Finance).WithMany(x => x.FinanceItems).HasForeignKey(x => x.FinanceId);
      //builder.HasOne(x => x.Finance).WithMany(x => x.FinanceItems).HasForeignKey(x => x.FinanceId);
      builder.HasOne(x => x.ReceivedUser).WithMany(x => x.ReceivedFinanceItems).HasForeignKey(x => x.ReceivedUserId);
      builder.HasOne(x => x.FinancialDocument).WithMany(x => x.FinanceItems).HasForeignKey(x => x.ExpenseFinancialDocumentId);
      builder.HasOne(x => x.PurchaseOrder).WithMany(x => x.FinanceItems).HasForeignKey(x => x.PurchaseOrderId);
      builder.HasOne(x => x.FinanceItemAllocationSummary).WithMany(x => x.FinanceItem).HasForeignKey(x => new
      {
        x.FinanceId,
        x.CooperatorId
      });
    }
  }
}