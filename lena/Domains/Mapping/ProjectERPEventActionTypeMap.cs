using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProjectERPEventActionTypeMap : IEntityTypeConfiguration<ProjectERPEventActionType>
  {
    public void Configure(EntityTypeBuilder<ProjectERPEventActionType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProjectERPEventActionTypes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Name);
      builder.HasRowVersion();
    }
  }
}