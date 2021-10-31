using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialPriceHistoryDetailMap : IEntityTypeConfiguration<BillOfMaterialPriceHistoryDetail>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterialPriceHistoryDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BillOfMaterialPriceHistoryDetails");
      builder.HasRowVersion();
      builder.HasOne(x => x.BillOfMaterialPriceHistory).WithMany(x => x.BillOfMaterialPriceHistoryDetails).HasForeignKey(x => x.BillOfMaerialPriceHistoryId);
    }
  }
}