using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class UnitTypeMap : IEntityTypeConfiguration<UnitType>
  {
    public void Configure(EntityTypeBuilder<UnitType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("UnitTypes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.IsActive);
      builder.HasRowVersion();
    }
  }
}