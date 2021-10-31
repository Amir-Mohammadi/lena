using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class TestOperationMap : IEntityTypeConfiguration<TestOperation>
  {
    public void Configure(EntityTypeBuilder<TestOperation> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("TestOperations");
      builder.Property(x => x.Id);
      builder.Property(x => x.Code);
      builder.Property(x => x.Name);
      builder.Property(x => x.Description);
      builder.HasRowVersion();
    }
  }
}
