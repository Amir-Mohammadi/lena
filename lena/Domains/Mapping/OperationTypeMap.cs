using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class OperationTypeMap : IEntityTypeConfiguration<OperationType>
  {
    public void Configure(EntityTypeBuilder<OperationType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("OperationTypes");
      builder.Property(x => x.Id).ValueGeneratedOnAdd();
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.Symbol).IsRequired();
      builder.HasRowVersion();
    }
  }
}
