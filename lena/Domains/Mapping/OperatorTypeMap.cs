using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OperatorTypeMap : IEntityTypeConfiguration<OperatorType>
  {
    public void Configure(EntityTypeBuilder<OperatorType> builder)
    {
      // builder.HasKey(x => x.Id);
      builder.ToTable("WorkStationParts_OperatorType");
      builder.Property(x => x.Id);
    }
  }
}
