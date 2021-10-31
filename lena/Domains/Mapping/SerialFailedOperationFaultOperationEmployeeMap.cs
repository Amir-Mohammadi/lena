using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SerialFailedOperationFaultOperationEmployeeMap : IEntityTypeConfiguration<SerialFailedOperationFaultOperationEmployee>
  {
    public void Configure(EntityTypeBuilder<SerialFailedOperationFaultOperationEmployee> builder)
    {
      builder.HasKey(x => new
      {
        x.SerialFailedOperationFaultOperationId,
        x.ProductionOperationEmployeeId
      });
      builder.ToTable("SerialFailedOperationFaultOperationEmployees");
      builder.Property(x => x.SerialFailedOperationFaultOperationId);
      builder.Property(x => x.ProductionOperationEmployeeId);
      builder.Property(x => x.ProductionOperatorEmployeeBanId).IsRequired(false);
      builder.HasRowVersion();
      builder.HasOne(x => x.SerialFailedOperationFaultOperation).WithMany(x => x.SerialFailedOperationFaultOperationEmployees).HasForeignKey(x => x.SerialFailedOperationFaultOperationId);//TODO fix it .WillCascadeOnDelete(true);
      builder.HasOne(x => x.ProductionOperationEmployee).WithMany(x => x.SerialFailedOperationFaultOperationEmployees).HasForeignKey(x => x.ProductionOperationEmployeeId);
      builder.HasOne(x => x.ProductionOperatorEmployeeBan).WithMany(x => x.SerialFailedOperationFaultOperationEmployees).HasForeignKey(x => x.ProductionOperatorEmployeeBanId);
    }
  }
}
