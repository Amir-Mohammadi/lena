using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CustomhouseMap : IEntityTypeConfiguration<Customhouse>
  {
    public void Configure(EntityTypeBuilder<Customhouse> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Customhouses");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.HasRowVersion();
    }
  }
}
