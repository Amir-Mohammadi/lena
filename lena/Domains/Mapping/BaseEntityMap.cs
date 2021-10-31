using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BaseEntityMap : IEntityTypeConfiguration<BaseEntity>
  {
    public void Configure(EntityTypeBuilder<BaseEntity> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).IsRequired().HasMaxLength(100);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.Description);
      builder.Property(x => x.EntityDescription);
      builder.Property(x => x.UserId);
      builder.Property(x => x.TransactionBatchId);
      builder.Property(x => x.FinancialTransactionBatchId);
      builder.HasRowVersion();
      builder.HasOne(x => x.TransactionBatch).WithOne(x => x.BaseEntity).HasForeignKey<BaseEntity>(x => x.TransactionBatchId);
      builder.HasOne(x => x.User).WithMany(x => x.BaseEntities).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.FinancialTransactionBatch).WithOne(x => x.BaseEntity).HasForeignKey<BaseEntity>(x => x.FinancialTransactionBatchId);
    }
  }
}