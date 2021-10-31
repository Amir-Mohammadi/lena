using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RepairProductionStuffDetailMap : IEntityTypeConfiguration<RepairProductionStuffDetail>
  {
    public void Configure(EntityTypeBuilder<RepairProductionStuffDetail> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ProductionStuffDetails_RepairProductionStuffDetail");
      builder.Property(x => x.Id);
      builder.Property(x => x.RepairProductionFaultId);
      builder.HasOne(x => x.RepairProductionFault).WithMany(x => x.RepairProductionStuffDetails).HasForeignKey(x => x.RepairProductionFaultId);
    }
  }
}
