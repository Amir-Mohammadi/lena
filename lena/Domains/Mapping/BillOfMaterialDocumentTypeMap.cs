using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BillOfMaterialDocumentTypeMap : IEntityTypeConfiguration<BillOfMaterialDocumentType>
  {
    public void Configure(EntityTypeBuilder<BillOfMaterialDocumentType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BillOfMaterialDocumentTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Description);
      builder.Property(x => x.Title).IsRequired();
      builder.HasRowVersion();
    }
  }
}
