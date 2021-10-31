using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPTaskDocumentMap : IEntityTypeConfiguration<ProjectERPTaskDocument>
  {
    public void Configure(EntityTypeBuilder<ProjectERPTaskDocument> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPTaskDocumentes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      //builder.Property(x => x.FileName);
      //builder.Property(x => x.FileFormat);
      //builder.Property(x => x.FileSize);
      //builder.Property(x => x.FileContent);
      builder.Property(x => x.ProjectERPTaskId);
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.CreatorUserId);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
      builder.HasOne(x => x.CreatorUser).WithMany(x => x.ProjectERPTaskDocuments).HasForeignKey(x => x.CreatorUserId);
      builder.HasOne(x => x.ProjectERPTask).WithMany(x => x.ProjectERPTaskDocuments).HasForeignKey(x => x.ProjectERPTaskId);
    }
  }
}