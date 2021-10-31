using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOperatorMap : IEntityTypeConfiguration<ProductionOperator>
  {
    public void Configure(EntityTypeBuilder<ProductionOperator> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionOperators");
      builder.Property(x => x.Id);
      builder.Property(x => x.MachineTypeOperatorTypeId);
      builder.Property(x => x.OperatorTypeId);
      builder.Property(x => x.OperationSequenceId);
      builder.Property(x => x.OperationId);
      builder.Property(x => x.DefaultTime);
      builder.Property(x => x.WrongLimitCount).IsRequired();
      builder.Property(x => x.ProductionOrderId);
      builder.HasRowVersion();
      builder.HasOne(x => x.OperationSequence).WithMany(x => x.ProductionOperators).HasForeignKey(x => x.OperationSequenceId);
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.ProductionOperators).HasForeignKey(x => x.ProductionOrderId);
      builder.HasOne(x => x.Operation).WithMany(x => x.ProductionOperators).HasForeignKey(x => x.OperationId);
      builder.HasOne(x => x.MachineTypeOperatorType).WithMany(x => x.ProductionOperators).HasForeignKey(x => x.MachineTypeOperatorTypeId);
      builder.HasOne(x => x.OperatorType).WithMany(x => x.ProductionOperators).HasForeignKey(x => x.OperatorTypeId);
    }
  }
}