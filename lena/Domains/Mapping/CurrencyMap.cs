using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CurrencyMap : IEntityTypeConfiguration<Currency>
  {
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Currencies");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.Sign).IsRequired();
      builder.Property(x => x.IsMain);
      builder.Property(x => x.DecimalDigitCount);
      builder.Property(x => x.Type);
      builder.HasRowVersion();
    }
  }
}
