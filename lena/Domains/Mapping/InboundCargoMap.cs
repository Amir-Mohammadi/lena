using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class InboundCargoMap : IEntityTypeConfiguration<InboundCargo>
  {
    public void Configure(EntityTypeBuilder<InboundCargo> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_InboundCargo");
      builder.Property(x => x.Id);
      builder.Property(x => x.BoxCount);
    }
  }
}
