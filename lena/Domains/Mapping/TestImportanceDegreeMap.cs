using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TestImportanceDegreeMap : IEntityTypeConfiguration<TestImportanceDegree>
  {
    public void Configure(EntityTypeBuilder<TestImportanceDegree> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TestImportanceDegrees");
      builder.Property(x => x.Id);
      builder.Property(x => x.Name);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
