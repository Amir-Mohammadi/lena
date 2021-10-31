using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionStepMap : IEntityTypeConfiguration<ProductionStep>
  {
    public void Configure(EntityTypeBuilder<ProductionStep> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionSteps");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.ProductivityImpactFactor);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
