using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOperatorMachineEmployeeMap : IEntityTypeConfiguration<ProductionOperatorMachineEmployee>
  {
    public void Configure(EntityTypeBuilder<ProductionOperatorMachineEmployee> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionOperatorMachineEmployees");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionOperatorId);
      builder.Property(x => x.MachineId);
      builder.Property(x => x.EmployeeId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.ProductionTerminalId);
      builder.HasOne(x => x.ProductionOperator).WithMany(x => x.ProductionOperatorMachineEmployees).HasForeignKey(x => x.ProductionOperatorId);
      builder.HasOne(x => x.Machine).WithMany(x => x.ProductionOperatorMachineEmployees).HasForeignKey(x => x.MachineId);
      builder.HasOne(x => x.ProductionTerminal).WithMany(x => x.ProductionOperatorMachineEmployees).HasForeignKey(x => x.ProductionTerminalId);
    }
  }
}
