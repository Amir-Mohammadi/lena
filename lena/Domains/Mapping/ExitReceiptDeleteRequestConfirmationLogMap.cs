using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ExitReceiptDeleteRequestConfirmationLogMap : IEntityTypeConfiguration<ExitReceiptDeleteRequestConfirmationLog>
  {
    public void Configure(EntityTypeBuilder<ExitReceiptDeleteRequestConfirmationLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ExitReceiptDeleteRequestConfirmationLogs");
      builder.Property(x => x.Id);
      builder.Property(x => x.ExitReceiptDeleteRequestId);
      builder.Property(x => x.ConfirmerUserId);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.Status);
      builder.HasRowVersion();
      builder.HasOne(x => x.ExitReceiptDeleteRequest).WithMany(x => x.ExitReceiptDeleteRequestConfirmationLogs).HasForeignKey(x => x.ExitReceiptDeleteRequestId);
      builder.HasOne(x => x.ConfirmerUser).WithMany(x => x.ExitReceiptDeleteRequestConfirmationLogs).HasForeignKey(x => x.ConfirmerUserId);
    }
  }
}
