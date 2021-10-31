using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SuppliesPurchaserUserMap : IEntityTypeConfiguration<SuppliesPurchaserUser>
  {
    public void Configure(EntityTypeBuilder<SuppliesPurchaserUser> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("SuppliesPurchaserUsers");
      builder.Property(x => x.Id);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsDefault);
      builder.Property(x => x.Description);
      builder.Property(x => x.PurchaserUserId);
      builder.HasRowVersion();
      builder.Property(x => x.StuffId);
      builder.HasOne(x => x.PurchaserUser).WithMany(x => x.SuppliesPurchaserUsers).HasForeignKey(x => x.PurchaserUserId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.SuppliesPurchaserUsers).HasForeignKey(x => x.StuffId);
    }
  }
}
