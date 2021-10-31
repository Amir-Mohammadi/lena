using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ReturnStoreReceiptMap : IEntityTypeConfiguration<ReturnStoreReceipt>
  {
    public void Configure(EntityTypeBuilder<ReturnStoreReceipt> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntities_ReturnStoreReceipt");
      builder.Property(x => x.Id);
    }
  }
}
