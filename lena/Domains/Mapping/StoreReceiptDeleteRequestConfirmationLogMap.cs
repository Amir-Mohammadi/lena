using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StoreReceiptDeleteRequestConfirmationLogMap : IEntityTypeConfiguration<StoreReceiptDeleteRequestConfirmationLog>
  {
    public void Configure(EntityTypeBuilder<StoreReceiptDeleteRequestConfirmationLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StoreReceiptDeleteRequestConfirmationLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.StoreReceiptDeleteRequestId);
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.Status);
      builder.HasRowVersion();
      builder.HasOne(x => x.StoreReceiptDeleteRequest).WithMany(x => x.StoreReceiptDeleteRequestConfirmationLogs).HasForeignKey(x => x.StoreReceiptDeleteRequestId);
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.StoreReceiptDeleteRequestConfirmationLogs).HasForeignKey(x => x.ConfirmerUserId);
    }
  }
}
