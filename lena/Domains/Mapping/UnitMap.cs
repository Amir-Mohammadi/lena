using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UnitMap : IEntityTypeConfiguration<Unit>
  {
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Units");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.IsMainUnit);
      builder.Property(x => x.DecimalDigitCount);
      builder.Property(x => x.ConversionRatio);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.UnitTypeId);
      builder.Property(x => x.Symbol);
      builder.HasRowVersion();
      builder.HasOne(x => x.UnitType).WithMany(x => x.Units).HasForeignKey(x => x.UnitTypeId);
    }
  }
}
