using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StockCheckingStuffMap : IEntityTypeConfiguration<StockCheckingStuff>
  {
    public void Configure(EntityTypeBuilder<StockCheckingStuff> builder)
    {
      builder.HasKey(x => new
      {
        x.StockCheckingId,
        x.StuffId
      });
      builder.ToTable("StockCheckingStuffs");
      builder.Property(x => x.StockCheckingId);
      builder.Property(x => x.StuffId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StockChecking).WithMany(x => x.StockCheckingStuffs).HasForeignKey(x => x.StockCheckingId);
      builder.HasOne(x => x.Stuff).WithMany(x => x.StockCheckingStuffs).HasForeignKey(x => x.StuffId);
    }
  }
}
