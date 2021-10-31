using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class NewShoppingDetailSummaryMap : IEntityTypeConfiguration<NewShoppingDetailSummary>
  {
    public void Configure(EntityTypeBuilder<NewShoppingDetailSummary> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("NewShoppingDetailSummaries");
      builder.Property(x => x.Id);
      builder.Property(x => x.QualityControlPassedQty);
      builder.Property(x => x.QualityControlFailedQty);
      builder.Property(x => x.QualityControlConsumedQty);
      builder.Property(x => x.NewShoppingDetailId);
      builder.HasRowVersion();
    }
  }
}