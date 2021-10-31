using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseOrderGroupMap : IEntityTypeConfiguration<PurchaseOrderGroup>
  {
    public void Configure(EntityTypeBuilder<PurchaseOrderGroup> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_PurchaseOrderGroup");
      builder.Property(x => x.Id);
    }
  }
}
