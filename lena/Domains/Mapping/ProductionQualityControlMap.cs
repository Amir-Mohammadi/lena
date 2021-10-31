using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionQualityControlMap : IEntityTypeConfiguration<ProductionQualityControl>
  {
    public void Configure(EntityTypeBuilder<ProductionQualityControl> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ProductionQualityControl");
      builder.Property(x => x.Id);
    }
  }
}
