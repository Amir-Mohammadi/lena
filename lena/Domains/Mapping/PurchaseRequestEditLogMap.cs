using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class PurchaseRequestEditLogMap : IEntityTypeConfiguration<PurchaseRequestEditLog>
  {
    public void Configure(EntityTypeBuilder<PurchaseRequestEditLog> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("PurchaseRequestEditLog");
      builder.Property(x => x.Id);
      builder.Property(x => x.AfterDeadLineDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.BeforeDeadLineDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.BeforeRequestQty);
      builder.Property(x => x.AfterRequestQty);
      builder.Property(x => x.Description);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.UserId);
      builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();
      builder.HasOne(x => x.User).WithMany(x => x.PurchaseRequestEditLogs).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.PurchaseRequest).WithMany(x => x.PurchaseRequestEditLogs).HasForeignKey(x => x.PurchaseRequestId);
    }
  }
}
