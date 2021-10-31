using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinancialTransactionTypeMap : IEntityTypeConfiguration<FinancialTransactionType>
  {
    public void Configure(EntityTypeBuilder<FinancialTransactionType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinancialTransactionTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.FinancialTransactionLevel);
      builder.Property(x => x.Factor);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.Property(x => x.RollbackFinancialTransactionTypeId);
      builder.HasOne(x => x.RollbackFinancialTransactionType).WithMany(x => x.ReferenceFinancialTransactionTypes).HasForeignKey(x => x.RollbackFinancialTransactionTypeId);
    }
  }
}
