using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReceiptMap : IEntityTypeConfiguration<Receipt>
  {
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_Receipt");
      builder.Property(x => x.Id);
      builder.Property(x => x.CooperatorId);
      builder.Property(x => x.Status);
      builder.Property(x => x.ReceiptDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.ReceiptType);
      builder.HasOne(x => x.Cooperator).WithMany(x => x.Receipts).HasForeignKey(x => x.CooperatorId);
    }
  }
}
