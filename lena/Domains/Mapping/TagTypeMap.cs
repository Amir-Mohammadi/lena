using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TagTypeMap : IEntityTypeConfiguration<TagType>
  {
    public void Configure(EntityTypeBuilder<TagType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TagTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name).IsRequired();
      builder.HasRowVersion();
    }
  }
}
