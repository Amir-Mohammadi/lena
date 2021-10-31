using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialPriceHistoryMap : IEntityTypeConfiguration<BillOfMaterialPriceHistory>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterialPriceHistory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BillOfMaterialPriceHistories");
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.Property(x => x.Version).IsRequired();
      builder.HasOne(x => x.User).WithMany(x => x.BillOfMaterialPriceHistories).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.BillOfMaterialPriceHistories).HasForeignKey(x => x.StuffId);
      builder.HasOne(x => x.Currency).WithMany(x => x.BillOfMaterialPriceHistories).HasForeignKey(x => x.CurrencyId);
    }
  }
}