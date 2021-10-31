using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReturnedExitReceiptRequestMap : IEntityTypeConfiguration<ReturnedExitReceiptRequest>
  {
    public void Configure(EntityTypeBuilder<ReturnedExitReceiptRequest> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ReturnedExitReceiptRequest");
      builder.Property(x => x.Id);
      builder.Property(x => x.ReturnStoreReceiptId);
      builder.HasOne(x => x.ReturnStoreReceipt).WithMany(x => x.ReturnedExitReceiptRequests).HasForeignKey(x => x.ReturnStoreReceiptId);
    }
  }
}
