using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class CooperatorFinancialAccountMap : IEntityTypeConfiguration<CooperatorFinancialAccount>
  {
    public void Configure(EntityTypeBuilder<CooperatorFinancialAccount> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("FinancialAccounts_CooperatorFinancialAccount");
      builder.Property(x => x.Id);
      builder.Property(x => x.CooperatorId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.CooperatorFinancialAccount).HasForeignKey(x => x.CooperatorId);
    }
  }
}
