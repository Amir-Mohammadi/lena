using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StockAdjustmentMap : IEntityTypeConfiguration<StockAdjustment>
  {
    public void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_StockAdjustment");
      builder.Property(x => x.Id);
      builder.Property(x => x.StockCheckingTagId);
      builder.Property(x => x.Amount);
      builder.Property(x => x.UnitId);
      builder.HasOne(x => x.StockCheckingTag).WithMany(x => x.StockAdjustments).HasForeignKey(x => x.StockCheckingTagId);
      builder.HasOne(x => x.Unit).WithMany(x => x.StockAdjustments).HasForeignKey(x => x.UnitId);
    }
  }
}
