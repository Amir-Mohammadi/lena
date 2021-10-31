using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionYearMap : IEntityTypeConfiguration<ProductionYear>
  {
    public void Configure(EntityTypeBuilder<ProductionYear> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionYears");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).HasMaxLength(1).IsFixedLength();
      builder.Property(x => x.Year).IsRequired();//TODO FixIt;//TODO fix it.HasColumnAnnotation("Range", new RangeAttribute(0000, 1500));
      builder.HasRowVersion();
    }
  }
}