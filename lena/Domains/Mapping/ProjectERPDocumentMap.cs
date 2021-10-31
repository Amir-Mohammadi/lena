using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPDocumentMap : IEntityTypeConfiguration<ProjectERPDocument>
  {
    public void Configure(EntityTypeBuilder<ProjectERPDocument> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPDocuments");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.DocumentId);
      builder.Property(x => x.UserId);
      builder.Property(x => x.Description);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.HasRowVersion();
      builder.HasOne(x => x.ProjectERP).WithMany(x => x.ProjectERPDocuments).HasForeignKey(x => x.ProjectERPId);
      builder.HasOne(x => x.ProjectERPDocumentType).WithMany(x => x.ProjectERPDocuments).HasForeignKey(x => x.ProjectERPDocumentTypeId);
    }
  }
}
