using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinanceAllocationMap : IEntityTypeConfiguration<FinanceAllocation>
  {
    public void Configure(EntityTypeBuilder<FinanceAllocation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinanceAllocations");
      builder.Property(x => x.FinanceId).IsRequired();
      builder.Property(x => x.PaymentMethod).IsRequired();
      builder.Property(x => x.Amount).IsRequired();
      builder.Property(x => x.ChequeNumber).IsRequired(false).HasMaxLength(70);
      builder.Property(x => x.AllocationDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.Property(x => x.Description).IsRequired(false);
      builder.HasOne(x => x.User).WithMany(x => x.FinanceAllocations).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Finance).WithMany(x => x.FinanceAllocations).HasForeignKey(x => x.FinanceId);//TODO fix it .WillCascadeOnDelete(true);
    }
  }
}