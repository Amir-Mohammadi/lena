using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProvisionersCartItemMap : IEntityTypeConfiguration<ProvisionersCartItem>
  {
    public void Configure(EntityTypeBuilder<ProvisionersCartItem> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProvisionersCartItems");
      builder.Property(x => x.ProviderId);
      builder.Property(x => x.RequestQty);
      builder.Property(x => x.SuppliedQty);
      builder.Property(x => x.Status);
      builder.HasRowVersion();
      builder.Property(x => x.ProvisionersCartId);
      builder.Property(x => x.PurchaseRequestId);
      builder.HasOne(x => x.ProvisionersCart).WithMany(x => x.ProvisionersCartItems).HasForeignKey(x => x.ProvisionersCartId);
      builder.HasOne(x => x.PurchaseRequest).WithOne(x => x.ProvisionersCartItem).HasForeignKey<ProvisionersCartItem>(x => x.PurchaseRequestId);
      builder.HasOne(x => x.Provider).WithMany(x => x.ProvisionersCartItems).HasForeignKey(x => x.ProviderId);
    }
  }
}