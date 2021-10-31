using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SerialFailedOperationMap : IEntityTypeConfiguration<SerialFailedOperation>
  {
    public void Configure(EntityTypeBuilder<SerialFailedOperation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SerialFailedOperations");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionOperationId).IsRequired();
      builder.Property(x => x.CreatedDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.Description).IsRequired(false);
      builder.Property(x => x.ConfirmUserId).IsRequired(false);
      builder.Property(x => x.ReviewerUserId).IsRequired(false);
      builder.Property(x => x.ReviewedDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.HasRowVersion();
      builder.HasOne(x => x.ReviewerUser).WithMany(x => x.ReviewerSerialFailedOperations).HasForeignKey(x => x.ReviewerUserId);
      builder.HasOne(x => x.ConfirmUser).WithMany(x => x.ConfirmSerialFailedOperations).HasForeignKey(x => x.ConfirmUserId);
      builder.HasOne(x => x.ProductionOperation).WithMany(x => x.SerialFailedOperations).HasForeignKey(x => x.ProductionOperationId);
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.SerialFailedOperations).HasForeignKey(x => x.ProductionOrderId);
      builder.HasOne(x => x.RepairProduction).WithMany(x => x.SerialFailedOperations).HasForeignKey(x => x.RepairProductionId);
    }
  }
}
