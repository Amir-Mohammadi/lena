using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomsDeclarationMap : IEntityTypeConfiguration<CustomsDeclaration>
  {
    public void Configure(EntityTypeBuilder<CustomsDeclaration> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_CustomsDeclaration");
      builder.Property(x => x.Id);
    }
  }
}
