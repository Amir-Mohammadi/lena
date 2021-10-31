using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StuffBasePriceTransportMap : IEntityTypeConfiguration<StuffBasePriceTransport>
  {
    public void Configure(EntityTypeBuilder<StuffBasePriceTransport> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StuffBasePriceTransports");
      builder.Property(x => x.Id);
      builder.Property(x => x.Type);
      builder.Property(x => x.ComputeType);
      builder.Property(x => x.Percent);
      builder.Property(x => x.Price);
      builder.Property(x => x.StuffBasePriceId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StuffBasePrice).WithOne(x => x.StuffBasePriceTransport).HasForeignKey<StuffBasePriceTransport>(x => x.StuffBasePriceId);
    }
  }
}