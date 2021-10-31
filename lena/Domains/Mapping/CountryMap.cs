using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CountryMap : IEntityTypeConfiguration<Country>
  {
    public void Configure(EntityTypeBuilder<Country> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Countries");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Title).IsRequired();
      builder.HasRowVersion();
    }
  }
}
