using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionRequestMap : IEntityTypeConfiguration<ProductionRequest>
  {
    public void Configure(EntityTypeBuilder<ProductionRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ProductionRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.DeadlineDate).HasColumnType("smalldatetime");
      builder.Property(x => x.CheckOrderItemId);
      builder.Property(x => x.Status);
      builder.HasOne(x => x.Unit).WithMany(x => x.ProductionRequests).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.CheckOrderItem).WithMany(x => x.ProductionRequests).HasForeignKey(x => x.CheckOrderItemId);
      builder.HasOne(x => x.ProductionRequestSummary).WithOne(x => x.ProductionRequest).HasForeignKey<ProductionRequestSummary>(x => x.ProductionRequestId);
    }
  }
}