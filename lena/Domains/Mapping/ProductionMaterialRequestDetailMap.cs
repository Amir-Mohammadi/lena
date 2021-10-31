using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionMaterialRequestDetailMap : IEntityTypeConfiguration<ProductionMaterialRequestDetail>
  {
    public void Configure(EntityTypeBuilder<ProductionMaterialRequestDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionMaterialRequestDetails");
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionOrder).WithMany(x => x.ProductionMaterialRequestDetails).HasForeignKey(x => x.ProductionOrderId);
      builder.HasOne(x => x.ProductionMaterialRequest).WithMany(x => x.ProductionMaterialRequestDetails).HasForeignKey(x => x.ProductionMaterialRequestId);
    }
  }
}