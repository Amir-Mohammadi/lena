using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StoreReceiptDeleteRequestMap : IEntityTypeConfiguration<StoreReceiptDeleteRequest>
  {
    public void Configure(EntityTypeBuilder<StoreReceiptDeleteRequest> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("StoreReceiptDeleteRequests");
      builder.Property(x => x.Id);
      builder.Property(x => x.Status);
      builder.Property(x => x.DateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.IsDelete);
      builder.Property(x => x.UserId);
      builder.Property(x => x.StoreReceiptId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.User).WithMany(x => x.StoreReceiptDeleteRequests).HasForeignKey(x => x.UserId);
      builder.HasOne(x => x.StoreReceipt).WithMany(x => x.StoreReceiptDeleteRequest).HasForeignKey(x => x.StoreReceiptId);
    }
  }
}