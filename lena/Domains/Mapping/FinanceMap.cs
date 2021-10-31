using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinanceMap : IEntityTypeConfiguration<Finance>
  {
    public void Configure(EntityTypeBuilder<Finance> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Finances");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.CooperatorId).IsRequired();
      builder.Property(x => x.FinanacialAccountDetailId).IsRequired(false);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.CurrencyId).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.LastConfimationId);
      builder.Property(x => x.Code).IsRequired(false).HasMaxLength(150);
      builder.Property(x => x.Description).IsRequired(false);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.FinancialAccountDetail).WithMany(x => x.Finances).HasForeignKey(x => x.FinanacialAccountDetailId);
      builder.HasOne(x => x.LatestFinanceConfirmation).WithOne(x => x.LatestFinance).HasForeignKey<Finance>(x => x.LastConfimationId);
      builder.HasOne(x => x.User).WithMany(x => x.Finances).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.Finances).HasForeignKey(x => x.CooperatorId);
      builder.HasOne(x => x.Currency).WithMany(x => x.Finances).HasForeignKey(x => x.CurrencyId);
    }
  }
}