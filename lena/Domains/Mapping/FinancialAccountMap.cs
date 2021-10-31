using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialAccountMap : IEntityTypeConfiguration<FinancialAccount>
  {
    public void Configure(EntityTypeBuilder<FinancialAccount> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialAccounts");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.CurrencyId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Currency).WithMany(x => x.FinancialAccounts).HasForeignKey(x => x.CurrencyId);
    }
  }
}
