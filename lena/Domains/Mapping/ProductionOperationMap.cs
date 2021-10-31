using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOperationMap : IEntityTypeConfiguration<ProductionOperation>
  {
    public void Configure(EntityTypeBuilder<ProductionOperation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionOperations");
      builder.HasSaveLog();
      builder.HasDescription();
      builder.HasRowVersion();
      builder.HasTransaction();
      builder.Property(x => x.Id);
      builder.Property(x => x.Time);
      builder.Property(x => x.ProductionId);
      builder.Property(x => x.ProductionOperatorId);
      builder.Property(x => x.FaildProductionOperationId);
      builder.Property(x => x.OperationId);
      builder.Property(x => x.ProductionTerminalId);
      builder.Property(x => x.TransactionBatchId);
      builder.Property(x => x.Status);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Qty);
      builder.Property(x => x.IsFaultCause);
      builder.HasOne(x => x.Production).WithMany(x => x.ProductionOperations).HasForeignKey(x => x.ProductionId);
      builder.HasOne(x => x.ProductionOperationEmployeeGroup).WithMany(x => x.ProductionOperations).HasForeignKey(x => x.ProductionOperationEmployeeGroupId);
      builder.HasOne(x => x.ProductionOperator).WithMany(x => x.ProductionOperations).HasForeignKey(x => x.ProductionOperatorId);
      builder.HasOne(x => x.Operation).WithMany(x => x.ProductionOperations).HasForeignKey(x => x.OperationId);
      builder.HasOne(x => x.ProductionTerminal).WithMany(x => x.ProductionOperations).HasForeignKey(x => x.ProductionTerminalId);
      builder.HasOne(x => x.FaildProductionOperation).WithOne(x => x.BaseProductionOperation).HasForeignKey<ProductionOperation>(x => x.FaildProductionOperationId);
      builder.HasOne(x => x.Decomposition).WithOne(x => x.ProductionOperation).HasForeignKey<Decomposition>(x => x.ProductionOperationId);
    }
  }
}