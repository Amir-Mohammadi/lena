using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class NewShoppingMap : IEntityTypeConfiguration<NewShopping>
  {
    public void Configure(EntityTypeBuilder<NewShopping> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_NewShopping");
      builder.Property(x => x.Id);
      builder.Property(x => x.QtyPerBox);
      builder.Property(x => x.BoxNo);
      builder.Property(x => x.LadingItemId);
      builder.HasOne(x => x.LadingItem).WithMany(x => x.NewShoppings).HasForeignKey(x => x.LadingItemId);
    }
  }
}
