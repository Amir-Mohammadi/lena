using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionLineWorkShiftMap : IEntityTypeConfiguration<ProductionLineWorkShift>
  {
    public void Configure(EntityTypeBuilder<ProductionLineWorkShift> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionLineWorkShifts");
      builder.Property(x => x.Id);
      builder.Property(x => x.WorkShiftId);
      builder.Property(x => x.ProductionLineId);
      builder.Property(x => x.SaveDate).HasColumnType("smalldatetime");
      builder.Property(x => x.FromDate).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionLine).WithMany(x => x.ProductionLineWorkShifts).HasForeignKey(x => x.ProductionLineId);
      builder.HasOne(x => x.WorkShift).WithMany(x => x.ProductionLineWorkShifts).HasForeignKey(x => x.WorkShiftId);
    }
  }
}
