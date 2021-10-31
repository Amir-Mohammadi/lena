using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class RialRateMap : IEntityTypeConfiguration<RialRate>
  {
    public void Configure(EntityTypeBuilder<RialRate> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("RialRates");
      builder.Property(x => x.Id);
      builder.Property(x => x.Rate);
      builder.Property(x => x.ReferenceRialRateId);
      builder.HasRowVersion();
      builder.Property(x => x.Amount);
      builder.Property(x => x.FinancialTransactionId);
      builder.Property(x => x.IsValid);
      builder.Property(x => x.IsUsed);
      builder.HasOne(x => x.ReferenceRialRate).WithMany(x => x.ReferencedRialRates).HasForeignKey(x => x.ReferenceRialRateId);
      builder.HasOne(x => x.FinancialTransaction).WithMany(x => x.RialRates).HasForeignKey(x => x.FinancialTransactionId);
    }
  }
}
