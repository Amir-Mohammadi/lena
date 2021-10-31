using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class FinanceConfirmationMap : IEntityTypeConfiguration<FinanceConfirmation>
  {
    public void Configure(EntityTypeBuilder<FinanceConfirmation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("FinanceConfirmations");
      builder.Property(x => x.Id);
      builder.Property(x => x.FinanceId).IsRequired();
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime").IsRequired();
      builder.Property(x => x.UserId).IsRequired();
      builder.Property(x => x.Status).IsRequired();
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.Finance).WithMany(x => x.FinanceConfirmations).HasForeignKey(x => x.FinanceId);//TODO fix it .WillCascadeOnDelete(true);
      builder.HasOne(x => x.User).WithMany(x => x.FinanceConfirmations).HasForeignKey(x => x.UserId);
    }
  }
}
