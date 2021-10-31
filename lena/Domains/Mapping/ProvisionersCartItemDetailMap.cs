using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProvisionersCartItemDetailMap : IEntityTypeConfiguration<ProvisionersCartItemDetail>
  {
    public void Configure(EntityTypeBuilder<ProvisionersCartItemDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProvisionersCartItemDetails");
      builder.Property(x => x.ProviderId);
      builder.Property(x => x.SupplyQty);
      builder.Property(x => x.ProvisionersCartItemId);
      builder.Property(x => x.Description);
      builder.Property(x => x.PurchaseOrderId);
      builder.HasRowVersion();
      builder.HasOne(x => x.ProvisionersCartItem).WithMany(x => x.ProvisionersCartItemDetails).HasForeignKey(x => x.ProvisionersCartItemId);
      builder.HasOne(x => x.Provider).WithMany(x => x.ProvisionersCartItemDetails).HasForeignKey(x => x.ProviderId);
      builder.HasOne(x => x.PurchaseOrder).WithOne().HasForeignKey<ProvisionersCartItemDetail>(x => x.PurchaseOrderId);
    }
  }
}