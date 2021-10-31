using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CargoMap : IEntityTypeConfiguration<Cargo>
  {
    public void Configure(EntityTypeBuilder<Cargo> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_Cargo");
      builder.Property(x => x.Id);
    }
  }
}
