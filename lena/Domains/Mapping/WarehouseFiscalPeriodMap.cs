using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarehouseFiscalPeriodMap : IEntityTypeConfiguration<WarehouseFiscalPeriod>
  {
    public void Configure(EntityTypeBuilder<WarehouseFiscalPeriod> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WarehouseFiscalPeriods");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.FromDate).HasColumnType("smalldatetime");
      builder.Property(x => x.ToDate).HasColumnType("smalldatetime");
      builder.Property(x => x.IsClosed);
      builder.Property(x => x.IsCurrent);
      builder.HasRowVersion();
    }
  }
}
