using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionOperationEmployeeGroupMap : IEntityTypeConfiguration<ProductionOperationEmployeeGroup>
  {
    public void Configure(EntityTypeBuilder<ProductionOperationEmployeeGroup> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionOperationEmployeeGroups");
      builder.Property(x => x.Id);
      builder.Property(x => x.HashedEmployee);
      builder.HasRowVersion();
    }
  }
}
