using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SerialFailedOperationFaultOperationMap : IEntityTypeConfiguration<SerialFailedOperationFaultOperation>
  {
    public void Configure(EntityTypeBuilder<SerialFailedOperationFaultOperation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SerialFailedOperationFaultOperations");
      builder.Property(x => x.Id);
      builder.Property(x => x.SerialFailedOperationId).IsRequired();
      builder.Property(x => x.OperationId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.SerialFailedOperation).WithMany(x => x.SerialFailedOperationFaultOperations).HasForeignKey(x => x.SerialFailedOperationId);
      builder.HasOne(x => x.Operation).WithMany(x => x.SerialFailedOperationFaultOperations).HasForeignKey(x => x.OperationId);
    }
  }
}
