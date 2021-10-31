using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class WarrantyExpirationExceptionTypeMap : IEntityTypeConfiguration<WarrantyExpirationExceptionType>
  {
    public void Configure(EntityTypeBuilder<WarrantyExpirationExceptionType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("WarrantyExpirationExceptionTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.FreeOfCharge).IsRequired();
      builder.HasRowVersion();
    }
  }
}
