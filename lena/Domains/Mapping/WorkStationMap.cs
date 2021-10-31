using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WorkStationMap : IEntityTypeConfiguration<WorkStation>
  {
    public void Configure(EntityTypeBuilder<WorkStation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WorkStations");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.ProductionLineId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionLine).WithMany(x => x.WorkStations).HasForeignKey(x => x.ProductionLineId);
    }
  }
}
