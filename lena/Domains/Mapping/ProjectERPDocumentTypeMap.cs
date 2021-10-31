using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPDocumentTypeMap : IEntityTypeConfiguration<ProjectERPDocumentType>
  {
    public void Configure(EntityTypeBuilder<ProjectERPDocumentType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPDocumentTypes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name);
      builder.Property(x => x.IsActive);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
