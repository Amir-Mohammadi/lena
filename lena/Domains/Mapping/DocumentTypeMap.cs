using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DocumentTypeMap : IEntityTypeConfiguration<DocumentType>
  {
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("DocumentTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
    }
  }
}
