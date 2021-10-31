using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class DocumentMap : IEntityTypeConfiguration<Document>
  {
    public void Configure(EntityTypeBuilder<Document> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("Documents");
      builder.Property(x => x.FileStream);
      builder.Property(x => x.CreationTime).HasColumnType("smalldatetime");
      builder.Property(x => x.FileSize);
      builder.Property(x => x.FileType);
      builder.Property(x => x.Name);
      builder.HasRowVersion();
    }
  }
}