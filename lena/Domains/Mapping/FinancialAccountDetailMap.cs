using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialAccountDetailMap : IEntityTypeConfiguration<FinancialAccountDetail>
  {
    public void Configure(EntityTypeBuilder<FinancialAccountDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialAccountDetails");
      builder.Property(x => x.Account).IsRequired();
      builder.Property(x => x.Type);
      builder.Property(x => x.AccountOwner).IsRequired();
      builder.Property(x => x.BankId).IsRequired();
      builder.Property(x => x.FinancialAccountId).IsRequired();
      builder.Property(x => x.IsArchive).IsRequired();
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.Bank).WithMany(x => x.FinancialAccountDetails).HasForeignKey(x => x.BankId);
      builder.HasOne(x => x.FinancialAccount).WithMany(x => x.FinancialAccountDetails).HasForeignKey(x => x.FinancialAccountId);
    }
  }
}
