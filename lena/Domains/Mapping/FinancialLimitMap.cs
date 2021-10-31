using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialLimitMap : IEntityTypeConfiguration<FinancialLimit>
  {
    public void Configure(EntityTypeBuilder<FinancialLimit> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialLimits");
      builder.Property(x => x.Id);
      builder.Property(x => x.Allowance);
      builder.Property(x => x.IsArchive);
      builder.Property(x => x.CurrencyId);
      builder.Property(x => x.UserId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Currency).WithMany(x => x.FinancialLimits).HasForeignKey(x => x.CurrencyId);
      builder.HasOne(x => x.User).WithMany(x => x.FinancialLimits).HasForeignKey(x => x.UserId);
    }
  }
}
