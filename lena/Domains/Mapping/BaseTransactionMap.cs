using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BaseTransactionMap : IEntityTypeConfiguration<BaseTransaction>
  {
    public void Configure(EntityTypeBuilder<BaseTransaction> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BaseTransactions");
      builder.Property(x => x.Id);
      builder.Property(x => x.Amount);
      builder.Property(x => x.EffectDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.Property(x => x.StuffSerialCode);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.TransactionBatchId);
      builder.Property(x => x.TransactionTypeId);
      builder.HasRowVersion();
      builder.Property(x => x.BillOfMaterialVersion);
      builder.Property(x => x.ReferenceTransactionId);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.WarehouseFiscalPeriodId);
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.IsEstimated);
      builder.HasOne(x => x.StuffSerial).WithMany(x => x.BaseTransactions).HasForeignKey(x => new
      {
        x.StuffSerialCode,
        x.StuffId
      });
      builder.HasOne(x => x.Unit).WithMany(x => x.BaseTransactions).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.TransactionType).WithMany(x => x.BaseTransactions).HasForeignKey(x => x.TransactionTypeId);
      builder.HasOne(x => x.TransactionBatch).WithMany(x => x.BaseTransactions).HasForeignKey(x => x.TransactionBatchId);
      builder.HasOne(x => x.BillOfMaterial).WithMany(x => x.BaseTransactions).HasForeignKey(x => new { x.BillOfMaterialVersion, x.StuffId });
      builder.HasOne(x => x.ReferenceTransaction).WithMany(x => x.ReferencedTransactions).HasForeignKey(x => x.ReferenceTransactionId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.BaseTransactions).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.BaseTransactions).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.WarehouseFiscalPeriod).WithMany(x => x.BaseTransactions).HasForeignKey(x => x.WarehouseFiscalPeriodId);
    }
  }
}