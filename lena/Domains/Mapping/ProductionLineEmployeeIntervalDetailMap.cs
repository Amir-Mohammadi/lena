using core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using core.Models;
using lena.Extensions;
using lena.Domains.Enums;
namespace lena.Domains.Files.Mappings
{
  public class ProductionLineEmployeeIntervalDetailMap : IEntityTypeConfiguration<ProductionLineEmployeeIntervalDetail>
  {
    public void Configure(EntityTypeBuilder<ProductionLineEmployeeIntervalDetail> builder)
    {
      builder.HasKey(x => x.Id);
      builder.ToTable("ProductionLineEmployeeIntervalDetails");
      builder.Property(x => x.Id);
      builder.Property(x => x.OperationId).IsRequired();
      builder.Property(x => x.Time).IsRequired();
      builder.Property(x => x.ProductionLineEmployeeIntervalId).IsRequired();
      builder.HasRowVersion();
      builder.HasOne(x => x.ProductionLineEmployeeInterval).WithMany(x => x.ProductionLineEmployeeIntervalDetails).HasForeignKey(x => x.ProductionLineEmployeeIntervalId);
      builder.HasOne(x => x.Operation).WithMany(x => x.ProductionLineEmployeeIntervalDetails).HasForeignKey(x => x.OperationId);
    }
  }
}
