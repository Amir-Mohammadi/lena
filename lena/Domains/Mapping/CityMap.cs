using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CityMap : IEntityTypeConfiguration<City>
  {
    public void Configure(EntityTypeBuilder<City> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Cities");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.CountryId);
      builder.Property(x => x.Title).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.Country).WithMany(x => x.Cities).HasForeignKey(x => x.CountryId);
    }
  }
}