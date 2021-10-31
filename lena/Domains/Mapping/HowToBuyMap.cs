using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class HowToBuyMap : IEntityTypeConfiguration<HowToBuy>
  {
    public void Configure(EntityTypeBuilder<HowToBuy> builder)
    {
      builder.HasKey(t => t.Id);
      builder.ToTable("HowToBuys");
      builder.Property(t => t.Id).ValueGeneratedOnAdd();
      builder.Property(t => t.Title).IsRequired();
      builder.Property(t => t.IsActive);
      builder.Property(t => t.RowVersion).IsRowVersion();
    }
  }
}