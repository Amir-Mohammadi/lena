using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPCategoryMap : IEntityTypeConfiguration<ProjectERPCategory>
  {
    public void Configure(EntityTypeBuilder<ProjectERPCategory> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPCategories");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name).IsRequired();
      builder.Property(x => x.IsActive);
      builder.Property(x => x.CreateDateTime).HasColumnType("smalldatetime");
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}