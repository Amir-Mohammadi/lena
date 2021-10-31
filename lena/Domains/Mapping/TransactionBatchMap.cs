using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TransactionBatchMap : IEntityTypeConfiguration<TransactionBatch>
  {
    public void Configure(EntityTypeBuilder<TransactionBatch> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TransactionBatches");
      builder.Property(x => x.Id);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.TransactionBatches).HasForeignKey(x => x.UserId);
    }
  }
}
