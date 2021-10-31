using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class StoreReceiptSerialProfileMap : IEntityTypeConfiguration<StoreReceiptSerialProfile>
  {
    public void Configure(EntityTypeBuilder<StoreReceiptSerialProfile> builder)
    {
      // builder.HasKey(x => new { x.Code, x.StuffId });
      builder.ToTable("SerialProfiles_StoreReceiptSerialProfile");
      builder.Property(x => x.Code);
      builder.Property(x => x.StuffId);
      builder.Property(x => x.StoreReceiptId);
    }
  }
}