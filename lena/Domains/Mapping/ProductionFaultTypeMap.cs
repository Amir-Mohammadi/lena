using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionFaultTypeMap : IEntityTypeConfiguration<ProductionFaultType>
  {
    public void Configure(EntityTypeBuilder<ProductionFaultType> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionFaultTypes");
      builder.Property(x => x.Id);
      builder.Property(x => x.Title).IsRequired();
      builder.Property(x => x.OperationId);
      builder.HasRowVersion();
      builder.HasOne(x => x.Operation).WithMany(x => x.ProductionFaultTypes).HasForeignKey(x => x.OperationId);
    }
  }
}
