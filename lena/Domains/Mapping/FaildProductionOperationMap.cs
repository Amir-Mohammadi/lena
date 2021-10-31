using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FaildProductionOperationMap : IEntityTypeConfiguration<FaildProductionOperation>
  {
    public void Configure(EntityTypeBuilder<FaildProductionOperation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FaildProductionOperations");
      builder.Property(x => x.Id);
      builder.Property(x => x.RepairProductionId);
      builder.Property(x => x.ReworkProductionOperationId);
      builder.HasRowVersion();
      builder.HasOne(x => x.RepairProduction).WithMany(x => x.FaildProductionOperations).HasForeignKey(x => x.RepairProductionId);
      builder.HasOne(x => x.ReworkProductionOperation).WithOne(x => x.ReworkFaildProductionOperation).HasForeignKey<FaildProductionOperation>(x => x.ReworkProductionOperationId);
    }
  }
}