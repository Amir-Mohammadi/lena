using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class BaseEntityDocumentTypeMap : IEntityTypeConfiguration<BaseEntityDocumentType>
  {
    public void Configure(EntityTypeBuilder<BaseEntityDocumentType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("BaseEntityDocumentTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Description);
      builder.Property(x => x.EntityType);
      builder.HasRowVersion();
    }
  }
}
