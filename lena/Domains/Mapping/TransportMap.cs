using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TransportMap : IEntityTypeConfiguration<Transport>
  {
    public void Configure(EntityTypeBuilder<Transport> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_Transport");
      builder.Property(x => x.Id);
      builder.Property(x => x.DriverName).IsRequired();
      builder.Property(x => x.PhoneNumber);
      builder.Property(x => x.CarNumber);
      builder.Property(x => x.CarInformation);
      builder.Property(x => x.ShippingCompanyName).IsRequired();
      builder.Property(x => x.TransportDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.TransportType);
      builder.Property(x => x.TransportDescription);
      builder.Property(x => x.OutputTransportId);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.EntranceTransport).WithOne(x => x.OutputTransport).HasForeignKey<Transport>(x => x.OutputTransportId);
    }
  }
}