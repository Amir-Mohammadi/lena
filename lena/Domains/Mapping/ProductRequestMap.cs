using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductRequestMap : IEntityTypeConfiguration<ProductRequest>
  {
    public void Configure(EntityTypeBuilder<ProductRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntities_ProductRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.Response);
    }
  }
}
