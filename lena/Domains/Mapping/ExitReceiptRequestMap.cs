using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ExitReceiptRequestMap : IEntityTypeConfiguration<ExitReceiptRequest>
  {
    public void Configure(EntityTypeBuilder<ExitReceiptRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ExitReceiptRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.Qty);
      builder.Property(x => x.UnitId);
      builder.Property(x => x.ExitReceiptRequestTypeId);
      builder.Property(x => x.WarehouseId);
      builder.Property(x => x.PriceAnnunciationItemId);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.Status);
      builder.Property(x => x.Address);
      builder.Property(x => x.CooperatorId);
      builder.HasOne(x => x.ExitReceiptRequestType).WithMany(x => x.ExitReceiptRequests).HasForeignKey(x => x.ExitReceiptRequestTypeId);
      builder.HasOne(x => x.Unit).WithMany(x => x.ExitReceiptRequests).HasForeignKey(x => x.UnitId);
      builder.HasOne(x => x.Warehouse).WithMany(x => x.ExitReceiptRequests).HasForeignKey(x => x.WarehouseId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.ExitReceiptRequests).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.ExitReceiptRequests).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.PriceAnnunciationItem).WithMany(x => x.ExitReceiptRequests).HasForeignKey(x => x.PriceAnnunciationItemId);
    }
  }
}
