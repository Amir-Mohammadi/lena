using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ScrumEntityDocumentMap : IEntityTypeConfiguration<ScrumEntityDocument>
  {
    public void Configure(EntityTypeBuilder<ScrumEntityDocument> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ScrumEntityDocuments");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.Description).IsRequired();
      builder.Property(x => x.ScrumEntityId);
      builder.Property(x => x.DocumentTypeId);
      builder.Property(x => x.DocumentId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.ScrumEntity).WithMany(x => x.ScrumEntityDocuments).HasForeignKey(x => x.ScrumEntityId);
      builder.HasOne(x => x.DocumentType).WithMany(x => x.ScrumEntityDocuments).HasForeignKey(x => x.DocumentTypeId);
    }
  }
}
