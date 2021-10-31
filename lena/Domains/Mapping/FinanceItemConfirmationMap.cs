using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinanceItemConfirmationMap : IEntityTypeConfiguration<FinanceItemConfirmation>
  {
    public void Configure(EntityTypeBuilder<FinanceItemConfirmation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinanceItemConfirmations");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinanceItemId).IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.Status).IsRequired();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.User).WithMany(x => x.FinanceItemConfirmations).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.FinanceItem).WithMany(x => x.FinanceItemConfirmations).HasForeignKey(x => x.FinanceItemId);//TODO fix it .WillCascadeOnDelete(true);
    }
  }
}
