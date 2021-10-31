using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StockCheckingPersonMap : IEntityTypeConfiguration<StockCheckingPerson>
  {
    public void Configure(EntityTypeBuilder<StockCheckingPerson> builder)
    {
      builder.HasKey(x => new
      {
        x.StockCheckingId,
        x.UserId
      });
      builder.ToTable("StockCheckingPersons");
      builder.Property(x => x.StockCheckingId);
      builder.Property(x => x.UserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.StockChecking).WithMany(x => x.StockCheckingPersons).HasForeignKey(x => x.StockCheckingId);
      builder.HasOne(x => x.User).WithMany(x => x.StockCheckingPersons).HasForeignKey(x => x.UserId);
    }
  }
}
