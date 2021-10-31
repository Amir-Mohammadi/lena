using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomQualityControlMap : IEntityTypeConfiguration<CustomQualityControl>
  {
    public void Configure(EntityTypeBuilder<CustomQualityControl> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_CustomQualityControl");
      builder.Property(x => x.Id);
    }
  }
}