using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TransactionTypeMap : IEntityTypeConfiguration<TransactionType>
  {
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TransactionTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Factor);
      builder.Property(x => x.TransactionLevel);
      builder.HasRowVersion();
      builder.Property(x => x.RollbackTransactionTypeId);
      builder.HasOne(x => x.RollbackTransactionType).WithMany(x => x.ReferenceTransactionTypes).HasForeignKey(x => x.RollbackTransactionTypeId);
    }
  }
}
