using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ForwarderMap : IEntityTypeConfiguration<Forwarder>
  {
    public void Configure(EntityTypeBuilder<Forwarder> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Forwarders");
      builder.Property(x => x.Id);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.Name);
      builder.HasRowVersion();
    }
  }
}
