using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RepairProductionFaultMap : IEntityTypeConfiguration<RepairProductionFault>
  {
    public void Configure(EntityTypeBuilder<RepairProductionFault> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_RepairProductionFault");
      builder.Property(x => x.Id);
      builder.Property(x => x.ProductionFaultTypeId);
      builder.Property(x => x.RepairProductionId);
      builder.HasOne(x => x.ProductionFaultType).WithMany(x => x.RepairProductionFaults).HasForeignKey(x => x.ProductionFaultTypeId);
      builder.HasOne(x => x.RepairProduction).WithMany(x => x.RepairProductionFaults).HasForeignKey(x => x.RepairProductionId);
    }
  }
}
