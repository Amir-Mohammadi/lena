using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionMaterialRequestMap : IEntityTypeConfiguration<ProductionMaterialRequest>
  {
    public void Configure(EntityTypeBuilder<ProductionMaterialRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ProductionMaterialRequest");
      builder.Property(x => x.Id);
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.ProductionMaterialRequests).HasForeignKey(x => x.ProductionOrderId);
    }
  }
}
