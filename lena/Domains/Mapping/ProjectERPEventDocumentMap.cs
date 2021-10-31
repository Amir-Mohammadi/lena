using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPEventDocumentMap : IEntityTypeConfiguration<ProjectERPEventDocument>
  {
    public void Configure(EntityTypeBuilder<ProjectERPEventDocument> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPEventDocumentes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      //builder.Property(x => x.FileName);
      //builder.Property(x => x.FileFormat);
      //builder.Property(x => x.FileSize);
      //builder.Property(x => x.FileContent);
      builder.Property(x => x.CreationDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CreatorUserId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.ProjectERPEventDocuments).HasForeignKey(x => x.CreatorUserId);
      builder.HasOne(x => x.ProjectERPEvent).WithMany(x => x.ProjectERPEventDocuments).HasForeignKey(x => x.ProjectERPEventId);
    }
  }
}