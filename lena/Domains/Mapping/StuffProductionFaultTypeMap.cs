using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffProductionFaultTypeMap : IEntityTypeConfiguration<StuffProductionFaultType>
  {
    public void Configure(EntityTypeBuilder<StuffProductionFaultType> builder)
    {
      builder.HasKey(x => new
      {
        x.ProductionFaultTypeId,
        x.StuffId
      });
      builder.ToTable("StuffProductionFaultTypes");
      builder.Property(x => x.ProductionFaultTypeId);
      builder.Property(x => x.StuffId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionFaultType).WithMany(x => x.StuffProductionFaultTypes).HasForeignKey(x => x.ProductionFaultTypeId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.StuffProductionFaultTypes).HasForeignKey(x => x.StuffId);
    }
  }
}
