using Microsoft.EntityFrameworkCore.Metadata.Builders;
using lena.Domains;
namespace lena.Extensions
{
  public static class ConfigurationExtentions
  {
    public static void HasDocument<TEntity>(this EntityTypeBuilder<TEntity> builder)
         where TEntity : class, IHasDocument
    {
      builder.Property(t => t.DocumentId).IsRequired();
      builder.HasOne(t => t.Document).WithOne().HasForeignKey<TEntity>(d => d.DocumentId);
    }
    public static void HasSaveLog<TEntity>(this EntityTypeBuilder<TEntity> builder)
         where TEntity : class, IHasSaveLog
    {
      builder.Property(t => t.DateTime);
      builder.Property(t => t.UserId);
      builder.HasOne(t => t.User).WithMany().HasForeignKey(d => d.UserId);
    }
    public static void HasTransaction<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasTransaction
    {
      builder.Property(t => t.TransactionBatchId);
      builder.HasOne(t => t.TransactionBatch).WithOne().HasForeignKey<TEntity>(d => d.TransactionBatchId);
    }
    public static void HasFinancialTransaction<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasFinancialTransaction
    {
      builder.Property(t => t.FinancialTransactionBatchId);
      builder.HasOne(t => t.FinancialTransactionBatch).WithOne().HasForeignKey<TEntity>(d => d.FinancialTransactionBatchId);
    }
  }
}