using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionLineEmployeeIntervalMap : IEntityTypeConfiguration<ProductionLineEmployeeInterval>
  {
    public void Configure(EntityTypeBuilder<ProductionLineEmployeeInterval> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionLineEmployeeIntervals");
      builder.Property(x => x.Id);
      builder.Property(x => x.RFId).IsRequired(false);
      builder.Property(x => x.ProductionLineId).IsRequired();
      builder.Property(x => x.EmployeeId).IsRequired();
      builder.Property(x => x.EntranceDateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.HashedOperation).IsRequired();
      builder.Property(x => x.StuffId).IsRequired();
      builder.Property(x => x.LastMoidfied).IsRequired(false);
      builder.Property(x => x.ExitDateTime).HasColumnType("smalldatetime").IsRequired(false);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionLine).WithMany(x => x.ProductionLineEmployeeIntervals).HasForeignKey(x => x.ProductionLineId);
      builder.HasOne(x => x.Employee).WithMany(x => x.ProductionLineEmployeeIntervals).HasForeignKey(x => x.EmployeeId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.ProductionLineEmployeeIntervals).HasForeignKey(x => x.StuffId);
    }
  }
}
