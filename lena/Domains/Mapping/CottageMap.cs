using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CottageMap : IEntityTypeConfiguration<Cottage>
  {
    public void Configure(EntityTypeBuilder<Cottage> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Cottages");
      builder.Property(x => x.Id);
      builder.Property(x => x.Index).IsRequired();
      builder.Property(x => x.CustomsDeclarationId);
      builder.HasRowVersion();
      builder.HasOne(x => x.CustomsDeclaration).WithMany(x => x.Cottages).HasForeignKey(x => x.CustomsDeclarationId);
    }
  }
}
