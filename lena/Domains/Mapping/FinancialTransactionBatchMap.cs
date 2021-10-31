using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialTransactionBatchMap : IEntityTypeConfiguration<FinancialTransactionBatch>
  {
    public void Configure(EntityTypeBuilder<FinancialTransactionBatch> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialTransactionBatches");
      builder.Property(x => x.Id);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.FinancialTransactionBatches).HasForeignKey(x => x.UserId);
    }
  }
}
