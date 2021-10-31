using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionSerialProfileMap : IEntityTypeConfiguration<ProductionSerialProfile>
  {
    public void Configure(EntityTypeBuilder<ProductionSerialProfile> builder)
    {
      // builder.HasKey(x => new { x.Code, x.StuffId });
      builder.ToTable("SerialProfiles_ProductionSerialProfile");
      builder.Property(x => x.Code);
      builder.Property(x => x.StuffId);
    }
  }
}