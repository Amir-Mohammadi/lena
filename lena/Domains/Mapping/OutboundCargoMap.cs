using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OutboundCargoMap : IEntityTypeConfiguration<OutboundCargo>
  {
    public void Configure(EntityTypeBuilder<OutboundCargo> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_OutboundCargo");
      builder.Property(x => x.Id);
    }
  }
}
