using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class SendProductMap : IEntityTypeConfiguration<SendProduct>
  {
    public void Configure(EntityTypeBuilder<SendProduct> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_SendProduct");
      builder.Property(x => x.Id);
      builder.Property(x => x.ExitReceiptId).IsRequired();
      builder.Property(x => x.PreparingSendingId);
      builder.HasOne(x => x.ExitReceipt).WithMany(x => x.SendProducts).HasForeignKey(x => x.ExitReceiptId);
    }
  }
}